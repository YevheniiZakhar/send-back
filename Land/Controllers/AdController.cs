using Azure;
using Land.Models.Help;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using MySqlConnector;
//using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
//using MySqlConnector;
//using System.Data;
//using System.Data.SqlClient;
//using MySqlConnector;

namespace Land.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<AdController> _logger;
        private readonly LandDbContext _context;

        public AdController(ILogger<AdController> logger, LandDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet("locality")]
        public async Task<IList<Localit>> Locality(string str) //[FromBody] GetAdRequest request
        {
            // TODO наполнить базу данных до 1000 обьявлений
            // TODO сделать привязку реакт к паганации на сервере через ентити фреймворк и таблицу в майсклюел обьявлений
            return await _context.Locality.FromSqlRaw("call GetLocality1({0})", str).ToListAsync();
        }

        [HttpPost("filter")]
        public PagedResult<Ad> Post([FromBody] GetAdRequest request) //[FromBody] GetAdRequest request
        {
                IQueryable<Ad> items = null;

            if (!string.IsNullOrEmpty(request.SearchInput))
            {
                items = _context.Ad.Where(i => i.Name.Contains(request.SearchInput));
            }

            if (request.SearchLocality != 0)
            {
                items = _context.Ad.Where(i => i.LocalityId == request.SearchLocality);
            }

            if (items == null)
                items = _context.Ad;

            var result = items.GetPaged(request.Page.Value, request.PageSize.Value);

            //var emailAddressParam = new SqlParameter("@str", "Киї");

            //var users = _context
            //            .Ad
            //            .FromSqlRaw("exec sp_GetUsers @emailAddress, @passwordHash", emailAddressParam)
            //            .ToList();
            // string sql = "EXEC GetLocality @str";

            // var str = new MySqlParameter("str", "Киї");
            // _context.Database.ExecuteSqlRaw()
            
            //var kkkk = _context.SomeModels.FromSqlRaw("CALL land.GetLocality1").ToList();
            //using (var con = new MySqlConnection("server=localhost;database=land;user=root;password=1234;port=3306;"))
            //{ // https://www.codeproject.com/Questions/3432607/Csharp-async-await-not-working-with-mysql
            //    con.Open();
            //    using (var cmd = new MySqlCommand("GetLocality", con))
            //    {
            //        var list = new List<string>();
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@str", "Киї");
            //        using (var reader = cmd.ExecuteReader())
            //        {
                        
            //            while (reader.Read())
            //            {
            //                list.Add(reader["Locality"].ToString());
            //            }
            //        }

            //        //var kkkk = reader.AsQueryable();
            //        using (var sda = new MySqlDataAdapter(cmd))
            //        {
            //            DataTable dt = new DataTable();
            //            var sss = new DataSet();
            //            sda.Fill(dt);   
            //            sda.Fill(sss);
            //            //dt.AsDataView().
            //            var ll = dt.AsEnumerable().Select(i => i.ItemArray[0]).ToList();
            //        }
            //    }
            //    con.Close();
            //}
            //var items = _context.Ad.ToList(); // GetPaged(2,3);
            //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "file-upload-article-cover.png");
            //var b = System.IO.File.ReadAllBytes(path);
            //items.ForEach(i => i.ByteFiles = b);
            return result;
        }

        //[HttpGet("locality")]
        //public IQueryable<Locality> Locality([FromQuery] string param) //[FromBody] GetAdRequest request
        //{
        //    var items = _context.Locality.AsQueryable();
            
        //    if (!string.IsNullOrEmpty(param))
        //    {
        //        return items.Where(i => i.Title.StartsWith(param));
        //    }

        //    return items;
        //}

        //api/ad/byId?id=1
        [HttpGet("byId")]
        public Ad GetById(int id)
        {
            // TODO не показывать район если это город по примеру Кривой Рог, Днепропетровская а не Кривой Рог, Криворожский, Днепропетровская
            var ad = _context.Ad.FirstOrDefault(x => x.Id == id);
            if (ad.LocalityId != 0)
            {
                var locality = _context.Locality.FromSqlRaw("call GetLocalityById({0})", ad.LocalityId).ToList();
                var response = new AdById(ad);
                response.Locality = locality.FirstOrDefault().Locality;
                return response;
            }
            
            return ad;
            // GetPaged(2,3);
            //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "file-upload-article-cover.png");
            //var b = System.IO.File.ReadAllBytes(path);
            //items.ForEach(i => i.ByteFiles = b);

        }

        [Route("data")]
        [HttpGet]
        public AdData GetData()
        {
            var data = new AdData
            {
                Category = _context.Category.ToList()
            }; 
            // GetPaged(2,3);
            //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "file-upload-article-cover.png");
            //var b = System.IO.File.ReadAllBytes(path);
            //items.ForEach(i => i.ByteFiles = b);
            return data;
        }

        [HttpPost]
        public ActionResult Post([FromForm] Ad ad)
        {
            try
             {
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        using (var ms = new MemoryStream()) // do async https://stackoverflow.com/questions/20805396/asynchronous-memory-streaming-approach-which-of-the-following
                        {
                            files[i].CopyTo(ms);
                            ad.GetType().GetProperty($"File{i + 1}").SetValue(ad, ms.ToArray());
                        }
                    }
                }
                ad.CreatedDate = DateTime.Now;
                _context.Ad.Add(ad);
                _context.SaveChanges();
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ad.FileName);

                //using (var stream = new FileStream(path, FileMode.Create))
                //{
                //    ad.FrontFile.CopyTo(stream);
                //}

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}