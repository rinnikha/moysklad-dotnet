using MoySklad.Api.Client;
using MoySklad.Api.Entities.Products;
using MoySklad.Api.Repositories.Base;

namespace MoySklad.Api.Repositories.Products;

public class ProductFolderRepository(ApiClient apiClient)
    : EntityRepository<ProductFolder>(apiClient, "entity/productfolder");