## Learning Management System

> Learning Management System မှာ အဓိက ရည်ရွယ်ချက်က Admins, Instructors, Students တို့ပါဝင်ပြီး Instructors တွေဆီက Courses တွေကို Students တွေ online ကနေရယူပြီး အလွယ်တကူ လေ့လာနိုင်ရန်ရည်ရွယ်ပြီး ဒီ system ကိုရေးသားထားပါတယ်

 ---------------------------------

Tech Stack

- Backend() - .Net(EFCore) API, MSSQL

Backend Repository ကို ဒီ Link(https://github.com/one-project-one-month/lms-csharp) ကနေ စမ်းလို့ရပါတယ်

---------------------------------

ပါ၀င်တဲ့ Table တွေကတော့
1. Roles
2. Users, Admins, Instructors, Students
3. Category, Courses, Lessons
4. Enrollments
5. Social Links

---
## Contributors

---

**Description**

Learning Management System ကို ကိုလင်းရဲ့ ဦးဆောင်မှုဖြင့် စတင်ခဲ့ပြီး တစ်လအတွင်းပြီးနိုင်‌‌လောက်သည်အထိ scope သတ်မှတ်ခဲ့ပါတယ်။ LMS မှာက ကျောင်းသားများအနေနဲ Online Courses တွေကို အလွယ်တကူ Courses တွေရယူ Enrollment လုပ်နိုင်ပြီး လွယ်လွယ်ကူကူနဲ မြန်မြန်ဆန်ဆန် သင်ခန်းစာတွေ online ကနေစာလေ့လာနိုင်ရန်ရည်ရွယ်ပြီး ရေးသားထားတာပါသည်။


### Roles
>  က Users တွေရဲ့ Roles တွေကိုသိမ်းထားပေးရန်

```
id int
role varchar(255)
```

### Users
>  Users က Admins, Instructors, Students ရဲ့ အချက်အလက်များ သိမ်းရန်၊ Roles နဲ့ Users ချိတ်ဆက်ပေးရန်

```
id int
role_id int 
username varchar(255) 
email varchar(255) 
password varchar(255) 
phone varchar(255) 
dob date 
address varchar(255) 
profile_photo varchar(255) 
is_available bit 
```

### Admins
> Admins က Users ထဲမှာရှိတဲ့ Admins တွေကိုသိမ်းထားရန်

```
id int
user_id int 
```

### Instructors
> Instructors က Users ထဲမှာရှိတဲ့ Instructors တွေကိုသိမ်းထားရန်

```
id int
user_id int 
nrc varchar(255) 
edu_background varchar(255) 
```

### Students
> Students က Users ထဲမှာရှိတဲ့ Students တွေကိုသိမ်းထားရန်

```
id int
user_id int 
```

### Category
> Category က Users တွေ Courses ရှာရတာလွယ်ကူဖို့ Category တွေကိုသိမ်းထားရန်

```
id int
name varchar(255)
```

### Courses
> Courses က Category ထဲမှာရှိတဲ့ Courses တွေကိုသိမ်းထားရန်

```
id int
instructor_id int 
category_id int 
courseName varchar(255) 
thumbnail varchar(255) 
is_available bit 
type varchar(255) 
level varchar(255) 
description varchar(255) 
duration datetime 
original_price decimal(18, 2)
current_price decimal(18, 2)
```

### Lessons
>  Lessons က Courses ထဲကလိုအပ်တဲ့ Lessons တွေကိုသိမ်းထားရန်

```
id int
course_id int 
title varchar(255) 
videoUrl varchar(255)
lessonDetail varchar(255) 
is_available bit 
```

### Enrollments
>  Enrollments က Students တွေက Courses တွေကို Enrollments အလွယ်တကူပို့ပေးနိုင်ရန်

```
id int
user_id int 
course_id int 
enrollment_date datetime 
is_completed bit 
completed_date datetime
```

### Social Links
>  Social Links က Users အချင်းချင်း contact လုပ်ရတာလွယ်ကူစေရန်

```
id int
course_id int 
facebook varchar(255)
X varchar(255)
telegram varchar(255)
phone varchar(255)
email varchar(255)
```

