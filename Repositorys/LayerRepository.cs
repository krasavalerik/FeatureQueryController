using WebApp.Entitys;

namespace WebApp.Repositorys
{
    public class LayerRepository : GenericRepository<Layer>
    {
        public LayerRepository(EfContext context) : base(context) { }

        public override Layer Get(long id)
        {
            return new Layer()
            {
                DefaultOpacity = 1,
                Id = 29,
                LayerOrder = 6,
                MapId = 1,
                Name = "Общая информация (демо)",
                Type = "geoserver",
                Url = "http://tap-demo:8082/geoserver/ows?namespace=GeneralInfo"
            };
        }
    }
}
