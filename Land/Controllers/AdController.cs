using System.Data;
using Land.Models.Help;
using Microsoft.AspNetCore.Mvc;

namespace Land.Controllers
{
  [ApiController]
  [Route("[controller]")]
  [Produces("application/json")]
  public class AdController : ControllerBase
  {
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
      // TODO ��������� ���� ������ �� 1000 ����������
      // TODO ������� �������� ����� � ��������� �� ������� ����� ������ ��������� � ������� � ��������� ����������
      return await _context.Locality.FromSqlRaw("call GetLocality({0})", str).ToListAsync();
    }

    [HttpGet("getlocalityById")]
    public async Task<IList<Localit>> GetLocalityById(string ids) //[FromBody] GetAdRequest request
    {
      // TODO ��������� ���� ������ �� 1000 ����������
      // TODO ������� �������� ����� � ��������� �� ������� ����� ������ ��������� � ������� � ��������� ����������
      return await _context.Locality.FromSqlRaw("call GetLocalityByIds({0})", ids).ToListAsync();
    }

    [HttpPost("filter")]
    public async Task<PagedResult<Ad>> Post([FromBody] GetAdRequest request) //[FromBody] GetAdRequest request
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

      var result = await items.GetPaged(request.Page.Value, request.PageSize.Value);
      var locIds = string.Join(",", result.Results.Select(i => i.LocalityId));
      var localities = await _context.Locality.FromSqlRaw("call GetLocalityByIds({0})", locIds).ToDictionaryAsync(x => x.Id, x => x.Locality);
      result.Results.ForEach(i => i.Locality = localities[i.LocalityId.Value]);

      return result;
    }

    [HttpGet("getById")]
    public async Task<Ad> GetById(int id)
    {
      // TODO �� ���������� ����� ���� ��� ����� �� ������� ������ ���, ���������������� � �� ������ ���, ������������, ����������������
      var ad = await _context.Ad.FirstOrDefaultAsync(x => x.Id == id);
      if (ad != null && ad.LocalityId != null && ad.LocalityId != 0)
      {
        var localityResult = _context.Locality.FromSqlRaw("call GetLocalityByIds({0})", ad.LocalityId).ToList();
        ad.Locality = localityResult?.FirstOrDefault()?.Locality;
        return ad;
      }

      return ad;

    }

    [HttpGet("getByUserEmail")]
    public async Task<List<Ad>> GetByUserId(string email)
    {
      // TODO �� ���������� ����� ���� ��� ����� �� ������� ������ ���, ���������������� � �� ������ ���, ������������, ����������������
      var ads = await _context.Ad.Where(x => x.UserEmail == email).ToListAsync();
      var locIds = string.Join(",", ads.Select(i => i.LocalityId));
      var localities = _context.Locality.FromSqlRaw("call GetLocalityByIds({0})", locIds).ToDictionary(x => x.Id, x => x.Locality);
      ads.ForEach(i => i.Locality = localities[i.LocalityId.Value]);

      return ads;
    }

    [Route("data")]
    [HttpGet]
    public AdData GetData()
    {
      var data = new AdData
      {
        Category = _context.Category.ToList()
      };

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
            // TODO async https://stackoverflow.com/questions/20805396/asynchronous-memory-streaming-approach-which-of-the-following
            using (var ms = new MemoryStream())
            {
              await files[i].CopyToAsync(ms);
              ad.GetType().GetProperty($"File{i + 1}")?.SetValue(ad, ms.ToArray());
            }
          }
        }
        ad.CreatedDate = DateTime.Now;
        await _context.Ad.AddAsync(ad);
        await _context.SaveChangesAsync();

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
            // TODO async https://stackoverflow.com/questions/20805396/asynchronous-memory-streaming-approach-which-of-the-following
            using (var ms = new MemoryStream())
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