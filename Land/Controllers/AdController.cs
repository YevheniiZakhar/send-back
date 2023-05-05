using Azure;
using Land.Models;
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
            return await _context.Locality.FromSqlRaw("call GetLocality({0})", str).ToListAsync();
        }

        [HttpGet("getlocalityById")]
        public async Task<IList<Localit>> GetLocalityById(string ids) //[FromBody] GetAdRequest request
        {
            // TODO наполнить базу данных до 1000 обьявлений
            // TODO сделать привязку реакт к паганации на сервере через ентити фреймворк и таблицу в майсклюел обьявлений
            return await _context.Locality.FromSqlRaw("call GetLocalityByIds({0})", ids).ToListAsync();
        }

        [HttpPost("filter")]
        public PagedResult<Ad> Post([FromBody] GetAdRequest request) //[FromBody] GetAdRequest request
        {
                IQueryable<Ad> items = _context.Ad.OrderByDescending(i => i.CreatedDate).Where(i => !i.Hidden);

            if (!string.IsNullOrEmpty(request.SearchInput))
            {
                items = items.Where(i => i.Name.Contains(request.SearchInput));
            }

            if (request.SearchLocality != 0)
            {
                items = items.Where(i => i.LocalityId == request.SearchLocality);
            }

            var result = items.GetPaged(request.Page.Value, request.PageSize.Value);
            var locIds = string.Join(",", result.Results.Select(i => i.LocalityId));
            var localities = _context.Locality.FromSqlRaw("call GetLocalityByIds({0})", locIds).ToDictionary(x => x.Id, x => x.Locality);
            result.Results.ForEach(i => i.Locality = localities[i.LocalityId.Value]);
            //locality.
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
        [HttpGet("getById")]
        public Ad GetById(int id)
        {
            // TODO не показывать район если это город по примеру Кривой Рог, Днепропетровская а не Кривой Рог, Криворожский, Днепропетровская
            var ad = _context.Ad.FirstOrDefault(x => x.Id == id);
            if (ad != null && ad.LocalityId != null && ad.LocalityId != 0)
            {
                var locality = _context.Locality.FromSqlRaw("call GetLocalityByIds({0})", ad.LocalityId).ToList();
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

        [HttpGet("getByUserEmail")]
        public async Task<List<Ad>> GetByUserId(string email)
        {
            // TODO не показывать район если это город по примеру Кривой Рог, Днепропетровская а не Кривой Рог, Криворожский, Днепропетровская
            var ads = await _context.Ad.Where(x => x.UserEmail == email).ToListAsync();
            var locIds = string.Join(",", ads.Select(i => i.LocalityId));
            var localities = _context.Locality.FromSqlRaw("call GetLocalityByIds({0})", locIds).ToDictionary(x => x.Id, x => x.Locality);
            ads.ForEach(i => i.Locality = localities[i.LocalityId.Value]);

            return ads;
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

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var item = await _context.Ad.FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return StatusCode(StatusCodes.Status400BadRequest);

            item.Hidden = !item.Hidden;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] Ad ad)
        {
            if (ad == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            try
             {
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        using (var ms = new MemoryStream()) // do async https://stackoverflow.com/questions/20805396/asynchronous-memory-streaming-approach-which-of-the-following
                        {
                            await files[i].CopyToAsync(ms);
                            ad.GetType().GetProperty($"File{i + 1}")?.SetValue(ad, ms.ToArray());
                        }
                    }
                }
                ad.CreatedDate = DateTime.Now;
                await _context.Ad.AddAsync(ad);
                await _context.SaveChangesAsync();
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

        [HttpPut]
        public async Task<ActionResult> Put([FromForm] Ad ad)
        {
            if (ad == null || ad.Id == 0) 
                return StatusCode(StatusCodes.Status500InternalServerError);
            try
            {
                var entity = await _context.Ad.FirstOrDefaultAsync(i => i.Id == ad.Id);

                if (entity == null) 
                    return StatusCode(StatusCodes.Status500InternalServerError);

                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        using (var ms = new MemoryStream()) // do async https://stackoverflow.com/questions/20805396/asynchronous-memory-streaming-approach-which-of-the-following
                        {
                            await files[i].CopyToAsync(ms);
                            ad.GetType().GetProperty($"File{i + 1}")?.SetValue(ad, ms.ToArray());
                        }
                    }

                    entity.File1 = ad.File1;
                    entity.File2 = ad.File2;
                    entity.File3 = ad.File3;
                    entity.File4 = ad.File4;
                    entity.File5 = ad.File5;
                    entity.File6 = ad.File6;
                    entity.File7 = ad.File7;
                    entity.File8 = ad.File8;
                }

                entity.Name = ad.Name;
                entity.Description = ad.Description;
                entity.Price = ad.Price;
                entity.CategoryId = ad.CategoryId;
                entity.Phone = ad.Phone;
                entity.UserName = ad.UserName;

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}