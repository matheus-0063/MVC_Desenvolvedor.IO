using AutoMapper;
using MFER.App.ViewModels;
using MFER.Business.Models;

namespace MFER.App.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
            CreateMap<Produto, ProdutoViewModel>().ReverseMap();

            /*Com essa configuração, o caminho é só um, tranformando Fornecedor
            em ForncerdorViewModel, caso queira fazer o caminho de volta, 
            precisamos utilizar o .ReverseMap();*/
        }
    }
}
