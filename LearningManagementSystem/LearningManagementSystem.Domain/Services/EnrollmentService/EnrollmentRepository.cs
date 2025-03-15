namespace LearningManagementSystem.Domain.Services.EnrollmentService;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _db;

    public EnrollmentRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<EnrollmentViewModels> CreateEnrollment(EnrollmentViewModels request)
    {
        try
        {
            request.updated_at = null; // Need to amend and take out

            var model = EnrollmentMapping(request);

            await _db.Enrollments.AddAsync(model);
            await _db.SaveChangesAsync();

            request = EnrollmentViewModelsMapping(model);

            return request;
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<EnrollmentViewModels>> GetEnrollments()
    {
        var model = await _db.Enrollments
            .AsNoTracking()
            .Where(x => x.isDeleted == false)
            .ToListAsync();

        var viewModels = model.Select(EnrollmentViewModelsMapping).ToList();

        return viewModels;
    }

    public async Task<List<EnrollmentViewModels>> GetEnrollment(int id)
    {
        var model = await _db.Enrollments
            .AsNoTracking()
            .Where(x => x.isDeleted == false && x.id == id)
            .ToListAsync();

        var viewModels = model.Select(EnrollmentViewModelsMapping).ToList();

        return viewModels;
    }

    public async Task<EnrollmentViewModels> UpdateEnrollment(int id, EnrollmentViewModels request)
    {
        var item = await _db.Enrollments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.id == id
                                 && x.isDeleted == false);
        if (item is null) { return null; }

        item = UpdateEnrollmentDetails(id, request, item); // Updating Users info

        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        var model = EnrollmentViewModelsMapping(item);

        return model;
    }

    public async Task<EnrollmentViewModels> PatchEnrollment(int id, EnrollmentViewModels request)
    {
        var item = await _db.Enrollments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.id == id
                                 && x.isDeleted == false);
        if (item is null) { return null; }

        item = UpdateEnrollmentDetails(id, request, item); // Updating Users info

        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        var model = EnrollmentViewModelsMapping(item);
        return model;
    }

    public async Task<bool?> DeleteEnrollment(int id)
    {
        var item = await _db.Enrollments
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


    private static TblEnrollments EnrollmentMapping(EnrollmentViewModels models)
    {
        return new TblEnrollments
        {
            id = models.id,
            user_id = models.user_id,
            course_id = models.course_id,
            enrollment_date = models.enrollment_date,
            is_completed = models.is_completed,
            completed_date = models.completed_date,
            created_at = models.created_at,
            updated_at = models.updated_at,
            isDeleted = false
        };
    }

    private static EnrollmentViewModels EnrollmentViewModelsMapping(TblEnrollments items)
    {
        return new EnrollmentViewModels
        {
            id = items.id,
            user_id = items.user_id,
            course_id = items.course_id,
            enrollment_date = items.enrollment_date,
            is_completed = items.is_completed,
            completed_date = items.completed_date,
            created_at = items.created_at,
            updated_at = items.updated_at,
            //isDeleted = false
        };
    }

    private static TblEnrollments UpdateEnrollmentDetails(int id, EnrollmentViewModels models, TblEnrollments item)
    {

        if (!string.IsNullOrEmpty(id.ToString()))
        {
            item.id = id;
        }
        if (!string.IsNullOrEmpty(item.user_id.ToString()))
        {
            item.user_id = models.user_id;
        }
        if (!string.IsNullOrEmpty(item.course_id.ToString()))
        {
            item.course_id = models.course_id;
        }
        if (!string.IsNullOrEmpty(models.enrollment_date.ToString()))
        {
            item.enrollment_date = models.enrollment_date;
        }
        item.is_completed = models.is_completed;
        item.completed_date = models.completed_date; // Don`t need because it is allowed null.

        if (!string.IsNullOrEmpty(models.created_at.ToString()))
        {
            item.created_at = models.created_at;
        }

        item.updated_at = DateTime.Now; // Need to update everytime

        return item;
    }
}