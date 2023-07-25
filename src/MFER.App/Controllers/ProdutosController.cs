using Microsoft.AspNetCore.Mvc;
using MFER.App.ViewModels;
using MFER.Business.Interfaces;
using AutoMapper;
using MFER.Business.Models;
using System.Security.Cryptography.Pkcs;

namespace MFER.App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;

        public ProdutosController(IProdutoRepository produtoRepository, IFornecedorRepository fornecedorRepository,IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores()));
            //Estou retornando uma lista de produtos com o seu devido Fornecedor
            //e mapeando na ProdutoViewModel
        }

        public async Task<IActionResult> Details(Guid id)
        {

            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var produtoViewModel = await PopularFornecedores(new ProdutoViewModel());
            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);
            if (!ModelState.IsValid) return View(produtoViewModel);

            var imgPrefixo = Guid.NewGuid() + "_"; 
            //Precisamos criar um Prefixo com Guid para a imagem pois cada imagem deve ser unica
            //Logo ficará o guid_nomeDoArquivo

            if(! await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return View(produtoViewModel); 
                // Estamos validando se o Upload foi bem sucedido
            }

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            //Passando o nome do arquivo que foi criado, para o parametro Imagem
            //Não irá mais da erro ao fazer o upload da imagem

            await _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(produtoViewModel);

            await _produtoRepository.Atualizar(_mapper.Map<Produto>(produtoViewModel));

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null) return NotFound();

            return View(produtoViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);
            if (produtoViewModel == null) return NotFound();

            await _produtoRepository.Remover(id);

            return RedirectToAction("Index");
        }
        
        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            var produto =  _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
            //Eu estou obtendo Um produto, com o id passado por parametro e mapeando
            //ele para ProdutoViewModel
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            // Na propriedade Fornecedores da ProdutoViewModel, eu estou obtendo
            // uma lista com de Fornecedores que tem aquele Produto
            return produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            // Na propriedade Fornecedores da ProdutoViewModel, eu estou obtendo
            // uma lista com de Fornecedores que tem aquele Produto
            return produtoViewModel;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;
            //Validando o tamanho do arquivo 

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefixo + arquivo.FileName);
            //Eu estou combinando o meu diretorio local (onde ta aplicação), o wwwroot (onde quero salvar a imagem), e o arquivo que eu quero subir

            if(System.IO.File.Exists(path)) 
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                //Validando se já existe um arquivo com este nome
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
                //CopyToAsync = ele o conteudo que está em arquivo para o disco
                // passando o parametro stream
                //stream = path e o diretorio de criação
            }

            return true;
        }
    }
}
