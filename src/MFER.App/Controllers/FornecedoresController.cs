using Microsoft.AspNetCore.Mvc;
using MFER.App.ViewModels;
using MFER.Business.Interfaces;
using AutoMapper;
using MFER.Business.Models;

namespace MFER.App.Controllers
{
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        //precisamos chamar o nosso repositorio para configurar os métodos
        private readonly IMapper _mapper;
        /*Os métodos do IRepository retornam as Models, mas nas Views precisam
         ser retornados as ViewModel, por isso precisamos do mapper para fazer 
         essa substituição*/
        private IEnderecoRepository _enderecoRepository;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, IMapper mapper, IEnderecoRepository enderecoRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _enderecoRepository = enderecoRepository;
        }


        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos()));
            // (await _fornecedorRepository.ObterTodos()) = vai retornar uma lista de Fornecedores
            // (_mapper.Map<IEnumerable<FornecedorViewModel>> == vai converter a lista de Fornecedores para uma lista de FornecedoresViewModel
        }


        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id); 
            //Usando o metodo auxiliar, podemos obter as informações do fornecedor

            if (fornecedorViewModel == null) return NotFound(); 
            //Caso o fornecedor seja nullo irá estourar uma exception

            return View(fornecedorViewModel);
            //Se o fornecedor não for nulo, ele retornará na view as informações 
            //que vierem de forncedorViewModel
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            //Aqui eu estou fazendo o caminho contrário do mapper
            //Existe o FornecedorViewModel passado pelo paramtro e eu quero 
            //mapear ele na Entidade

            await _fornecedorRepository.Adicionar(fornecedor);
            //No repositorio de Fornecedor eu vou Adicionar a variavel forncedor
            //com as informações necessárias

            return RedirectToAction("Index");
            //Após adicionar o Forncedor vai retornar para a pagina Index
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorProdutosEndereco(id);
            //Usando o metodo auxiliar, podemos obter as informações do fornecedor

            if (fornecedorViewModel == null) return NotFound();
            //Caso o fornecedor seja nullo irá estourar uma exception

            return View(fornecedorViewModel);
            //Se o fornecedor não for nulo, ele retornará na view as informações 
            //que vierem de forncedorViewModel
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return NotFound();
            //Conferindo se o id passado pelo parametro é igual o id da ViewModel

            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorRepository.Atualizar(fornecedor);
            //No repositorio de Fornecedor eu vou Atualizar a variavel fornecedor
            //com as informações necessárias

            return RedirectToAction("Index");
            //Após atualizar o Forncedor vai retornar para a pagina Index
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            return View(fornecedorViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorRepository.Remover(id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if(fornecedor == null) return NotFound();

            return PartialView("_DetalhesEndereco", fornecedor);
        }

        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            //Método que irá trazer as informações atuais para a modal
            var fornecedor = await ObterFornecedorEndereco(id);
            //colocando na variavel fornecedor as informações do fornecedor
            //com o endereço passados pelos ID

            if (fornecedor == null) return NotFound();

            return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco });
            //E retornando na PartialView (_AtualizarEndereco) uma nova instancia de ForncedorViewModel,
            //mas populando apenas a propriedade Endereco com o conteudo que veio da variaavel fornecedor
            //retorna via PartialView, quando eu fizer a chamada via Jquery ele irá me retornar só a partialview
            //como Modal
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorViewModel);

            await _enderecoRepository.Atualizar(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));
            
            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorViewModel.Endereco.FornecedorId });
            //Precisamos montar a URL, com o método Action informando a ação (ObterEndereco),
            //falando em qual controller (Fornecedores), e o FornecedorId
            //Isso irá para um link Json

            return Json(new { sucess = true, url });
        }

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
            //método auxiliar, qualquer método da Controller que eu
            //tiver que buscar o FornecedorEndereco eu posso usar este método
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
            // (await _fornecedorRepository.ObterFornecedorEndereco(id)) = To obtendo um Fornecedor e Endereco pelo Id
            // _mapper.Map<FornecedorViewModel> = Convertendo para FornecedorViewModel o resultado da busca acima
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        //método auxiliar, qualquer método da Controller que eu
        //tiver que buscar o FornecedorEndereco eu posso usar este método
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
            // (await _fornecedorRepository.ObterFornecedorProdutosEndereco(id)) = To obtendo um Fornecedor, Produtos e Endereco pelo Id
            // _mapper.Map<FornecedorViewModel> = Convertendo para FornecedorViewModel o resultado da busca acima
        }


    }
}
