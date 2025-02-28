namespace LearningManagementSystem.Domain.Services.UsersServices;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public UsersViewModels CreateUser(UsersViewModels user)
    {
        user.updated_at = null; // Need to amend and take out

        TblUsers userModel = UsersMapping(user);

        _db.Users.Add(userModel);
        _db.SaveChanges();

        return user;
    }

    public List<UsersViewModels> GetInstructors()
    {
        var model = _db.Users
            .AsNoTracking()
            .Where(x => // x.Role == "Instructor" && // need to check with roles
            x.isDeleted == false)
            .ToList();

        var userViewModels = model.Select(UsersViewModelsMapping).ToList();

        return userViewModels;
    }

    public List<UsersViewModels> GetStudents()
    {
        var model = _db.Users
            .AsNoTracking()
            .Where(x => //x.Role == "Student" && // need to check with roles
            x.isDeleted == false)
            .ToList();

        var userViewModels = model.Select(UsersViewModelsMapping).ToList();

        return userViewModels;
    }

    public List<UsersViewModels> GetInstructor(int id)
    {
        var model = _db.Users
            .AsNoTracking()
            .Where(x => //x.Role == "Instructor"&& // need to check with roles
            x.isDeleted == false && x.id == id) 
            .ToList();

        var userViewModels = model.Select(UsersViewModelsMapping).ToList();

        return userViewModels;
    }

    public List<UsersViewModels> GetStudent(int id)
    {
        var model = _db.Users
            .AsNoTracking()
            .Where(x => x.isDeleted == false && x.id == id) // need to check with roles
            .ToList();

        var userViewModels = model.Select(UsersViewModelsMapping).ToList();

        return userViewModels;
    }

    public UsersViewModels? UpdateUser(int id, UsersViewModels user)
    {
        var item = _db.Users
            .AsNoTracking()
            .FirstOrDefault(x => x.id == id 
            && x.isDeleted == false);
        if (item is null) { return null; }

        item = UpdateUserDetails(id, user, item); // Updating Users info

        _db.Entry(item).State = EntityState.Modified;
        _db.SaveChanges();

        var model = UsersViewModelsMapping(item);

        return model;
    }

    public UsersViewModels? PatchUser(int id, UsersViewModels user)
    {
        var item = _db.Users
            .AsNoTracking()
            .FirstOrDefault(x => x.id == id
            && x.isDeleted == false);
        if (item is null) { return null; }

        item = UpdateUserDetails(id, user, item); // Updating Users info

        _db.Entry(item).State = EntityState.Modified;
        _db.SaveChanges();

        var model = UsersViewModelsMapping(item);
        return model;
    }

    public bool? DeleteUser(int id)
    {
        var item = _db.Users
            .AsNoTracking()
            .FirstOrDefault(x => x.id == id 
            && x.isDeleted == false);
        if (item is null)
        {
            return null;
        }

        item.isDeleted = true;

        _db.Entry(item).State = EntityState.Modified;
        var result = _db.SaveChanges();

        return result > 0;
    }

    // Can use for instructor and students
    private static TblUsers UsersMapping(UsersViewModels user)
    {
        return new TblUsers
        {
            //id = Guid.NewGuid(), 
            id =  0, 
            username = user.username,
            email = user.email,
            password = user.password,
            phone = user.phone,
            dob = user.dob,
            address = user.address,
            profile_photo = user.profile_photo,
            role_id = user.role_id,
            is_available = user.is_available,
            created_at = user.created_at,
            updated_at = user.updated_at,
            isDeleted = false
        };
    }

    private static UsersViewModels UsersViewModelsMapping(TblUsers user)
    {
        return new UsersViewModels
        {

            //UserId = user.UserId,
            username = user.username,
            email = user.email,
            password = user.password,
            phone = user.phone,
            dob = user.dob,
            address = user.address,
            profile_photo = user.profile_photo,
            role_id = user.role_id,
            is_available = user.is_available,
            created_at = user.created_at,
            updated_at = user.updated_at
            //isDeleted = false
        };
    }

    private static TblUsers UpdateUserDetails(int id, UsersViewModels user, TblUsers item)
    {

        if (!string.IsNullOrEmpty(id.ToString()))
        {
            item.id = id;
            //item.id = Guid.Parse(id);
        }
        if (!string.IsNullOrEmpty(user.role_id.ToString()))
        {
            item.role_id = user.role_id;
        }
        if (!string.IsNullOrEmpty(user.username))
        {
            item.username = user.username;
        }
        if (!string.IsNullOrEmpty(user.email))
        {
            item.email = user.email;
        }
        if (!string.IsNullOrEmpty(user.password))
        {
            item.password = user.password;
        }
        if (!string.IsNullOrEmpty(user.phone))
        {
            item.phone = user.phone;
        }
        if (!string.IsNullOrEmpty(user.dob.ToString()))
        {
            item.dob = user.dob;
        }
        if (!string.IsNullOrEmpty(user.address))
        {
            item.address = user.address;
        }
        if (!string.IsNullOrEmpty(user.profile_photo))
        {
            item.profile_photo = user.profile_photo;
        }
        if (!string.IsNullOrEmpty(user.is_available.ToString()))
        {
            item.is_available = user.is_available;
        }
        if (!string.IsNullOrEmpty(user.created_at.ToString()))
        {
            item.created_at = user.created_at;
        }

        item.updated_at = DateTime.Now; // Need to update everytime
        
        return item;
    }

}