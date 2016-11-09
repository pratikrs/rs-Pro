using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using UOWFirstDemo.DataAccess.RepositoryPattern;
using UOWFirstDemo.DataContent;
using UOWFirstDemo.Entities;

namespace UOWFirstDemo.DataAccess.GenericRepositiry
{
    public class StudentRepository : Repository<StudentInfo>
    {
        private DemoDBEntities1 _context = null;
        public StudentRepository(DemoDBEntities1 context, UnitOfWork _uom)
            : base(context, _uom)
        {
            _context = context;
        }

        public List<StudentModel> GetAllPassengers()
        {
            return base.GetAll().Select(p => new StudentModel
            {
                Id = p.Id,
                Phone = p.Phone,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                CountryId = p.CountryId ?? 0,
                StateId = p.StateId ?? 0,
                AddressInfoes = p.AddressInfoes.Select(a => new AddressModel
                {
                    Address1 = a.Address1,
                    Address2 = a.Address2,
                    Id = a.Id,
                    StudentInfoId = a.StudentInfoId
                }).ToList(),
                ContactInfoes = p.ContactInfoes.Select(c => new ContactModel
                {
                    Id = c.Id,
                    Phone1 = c.Phone1,
                    StudentInfoId = c.StudentInfoId
                }).ToList()
            }).ToList();
        }

        public StudentModel GetStudentById(int studentId)
        {
            var studentInfo = base.Get(x => x.Id == studentId);
            return new StudentModel
            {
                Id = studentInfo.Id,
                Phone = studentInfo.Phone,
                FirstName = studentInfo.FirstName,
                LastName = studentInfo.LastName,
                Email = studentInfo.Email,
                CountryId = studentInfo.CountryId ?? 0,
                StateId = studentInfo.StateId ?? 0,
                AddressInfoes = studentInfo.AddressInfoes.Select(a => new AddressModel
                {
                    Address1 = a.Address1,
                    Address2 = a.Address2,
                    Id = a.Id,
                    StudentInfoId = a.StudentInfoId
                }).ToList(),
                ContactInfoes = studentInfo.ContactInfoes.Select(c => new ContactModel
                {
                    Id = c.Id,
                    Phone1 = c.Phone1,
                    StudentInfoId = c.StudentInfoId
                }).ToList()
            };

        }

        public void InsertStudent(StudentModel student)
        {
            var dbset = _context.Set<StudentInfo>();
            StudentInfo _student = new StudentInfo();
            _student.Id = student.Id;
            _student.FirstName = student.FirstName;
            _student.LastName = student.LastName;
            _student.Phone = student.Phone;
            _student.Email = student.Email;
            _student.CountryId = student.CountryId;
            _student.StateId = _student.CountryId = student.CountryId;
            foreach (var contact in student.ContactInfoes)
            {
                _student.ContactInfoes.Add(new ContactInfo { Id = contact.Id, Phone1 = contact.Phone1 });
            }
            foreach (var address in student.AddressInfoes)
            {
                _student.AddressInfoes.Add(new AddressInfo { Address1 = address.Address1, Address2 = address.Address2 });
            }

            dbset.Add(_student);
            _context.SaveChanges();
        }
        public void UpdateStudent(StudentModel student)
        {
            var studentInfo = base.Get(x => x.Id == student.Id);
            var dbset = _context.Set<StudentInfo>();
            //StudentInfo _student = new StudentInfo();
            studentInfo.Id = student.Id;
            studentInfo.FirstName = student.FirstName;
            studentInfo.LastName = student.LastName;
            studentInfo.Phone = student.Phone;
            studentInfo.Email = student.Email;
            studentInfo.CountryId = student.CountryId;
            studentInfo.StateId = studentInfo.CountryId = student.CountryId;
            //_context.Entry(studentInfo).State = EntityState.Modified;
            //foreach (var contact in student.ContactInfoes)
            //{
            //    _student.ContactInfoes.Add(new ContactInfo { Id = contact.Id, Phone1 = contact.Phone1 });
            //}

            base.Attach(studentInfo);
            //_context.SaveChanges();
        }

        public void DeleteStudent(int studentId)
        {
            //    var student = _context.Set<StudentInfo>().Include(m => m.AddressInfoes).Include(x=>x.ContactInfoes)
            //.SingleOrDefault(m => m.Id == studentId);
            var student = _context.StudentInfoes.Find(studentId);
            base.Delete(student);
            //_context.StudentInfoes.Remove(student);
        }

    }
}
