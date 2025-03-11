namespace LearningManagementSystem.Domain.Services.Social_linksService;

public class Social_linksRepository : ISocial_linksRepository
{
    private readonly AppDbContext _db;

    public Social_linksRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Social_linksViewModels> CreateSocial_links(Social_linksViewModels requests)
    {
        try
        {
            requests.updated_at = null; // Need to amend and take out

            var model = Social_linksMapping(requests);

            await _db.Social_Links.AddAsync(model);
            await _db.SaveChangesAsync();

            requests = Social_linksViewModelsMapping(model);

            return requests;
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<Social_linksViewModels>> GetSocial_links()
    {
        var model = await _db.Social_Links
            .AsNoTracking()
            .Where(x => x.isDeleted == false)
            .ToListAsync();

        var viewModels = model.Select(Social_linksViewModelsMapping).ToList();

        return viewModels;
    }

    public async Task<List<Social_linksViewModels>> GetSocial_linksById(int id)
    {
        var model = await _db.Social_Links
            .AsNoTracking()
            .Where(x => x.isDeleted == false && x.id == id)
            .ToListAsync();

        var ViewModels = model.Select(Social_linksViewModelsMapping).ToList();

        return ViewModels;
    }

    public async Task<Social_linksViewModels> UpdateSocial_links(int id, Social_linksViewModels requests)
    {
        var item = await _db.Social_Links
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.id == id
                                 && x.isDeleted == false);
        if (item is null) { return null; }

        item = UpdateSocial_linksDetails(id, requests, item);

        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        var model = Social_linksViewModelsMapping(item);

        return model;
    }

    public async Task<Social_linksViewModels> PatchSocial_links(int id, Social_linksViewModels requests)
    {
        var item = await _db.Social_Links
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.id == id
                                 && x.isDeleted == false);
        if (item is null) { return null; }

        item = UpdateSocial_linksDetails(id, requests, item); // Updating Users info

        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        var model = Social_linksViewModelsMapping(item);
        return model;
    }

    public async Task<bool?> DeleteSocial_links(int id)
    {
        var item = await _db.Social_Links
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.id == id
                                 && x.isDeleted == false);
        if (item is null)
        {
            return null;
        }

        item.isDeleted = true;

        _db.Entry(item).State = EntityState.Modified;
        var result = await _db.SaveChangesAsync();

        return result > 0;
    }

    // Can use for instructor and students
    private static TblSocial_Links Social_linksMapping(Social_linksViewModels requests)
    {
        return new TblSocial_Links
        {
            id = requests.id,
            course_id = requests.course_id,
            facebook = requests.facebook,
            X = requests.X,
            telegram = requests.telegram,
            phone = requests.phone,
            email = requests.email,
            created_at = requests.created_at,
            updated_at = requests.updated_at,
            isDeleted = false
        };
    }

    private static Social_linksViewModels Social_linksViewModelsMapping(TblSocial_Links requests)
    {
        return new Social_linksViewModels
        {
            id = requests.id,
            course_id = requests.course_id,
            facebook = requests.facebook,
            X = requests.X,
            telegram = requests.telegram,
            phone = requests.phone,
            email = requests.email,
            created_at = requests.created_at,
            updated_at = requests.updated_at,
            //isDeleted = false
        };
    }

    private static TblSocial_Links UpdateSocial_linksDetails(int id, Social_linksViewModels requests, TblSocial_Links item)
    {

        if (!string.IsNullOrEmpty(id.ToString()))
        {
            //item.Category_Id = Guid.Parse(id);
            item.id = id;
        }
        if (!string.IsNullOrEmpty(requests.course_id.ToString()))
        {
            item.course_id = requests.course_id;
        }
        if (!string.IsNullOrEmpty(requests.facebook))
        {
            item.facebook = requests.facebook;
        }
        if (!string.IsNullOrEmpty(requests.X))
        {
            item.X = requests.X;
        }
        if (!string.IsNullOrEmpty(requests.telegram))
        {
            item.telegram = requests.telegram;
        }
        if (!string.IsNullOrEmpty(requests.phone))
        {
            item.phone = requests.phone;
        }
        if (!string.IsNullOrEmpty(requests.email))
        {
            item.email = requests.email;
        }
        if (!string.IsNullOrEmpty(requests.created_at.ToString()))
        {
            item.created_at = requests.created_at;
        }

        item.updated_at = DateTime.Now; // Need to update everytime

        return item;
    }
}