using MyDBFDemo.DataAccess.GenericRepository;
using MyDBFDemo.DbContent;
using MyDBFDemo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDBFDemo.DataAccess.RepositoryPattern
{
    //class StudentRepository
    //{
    //}

    public class StudentRepository : Repository<StudentInfo>
    {
        private DBFDemoEntities _context = null;
        public StudentRepository(DBFDemoEntities context, UnitOfWork _uom)
            : base(context, _uom)
        {
            _context = context;
        }

        public List<StudentEntity> GetAllPassengers()
        {
            return base.GetAll().Select(p => new StudentEntity
            {
                Id = p.Id,
                Phone = p.Phone,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                Gender = p.Gender,
                CountryId = p.CountryId ?? 0,
                StateId = p.StateId ?? 0
            }).ToList();
        }

        public void AddPassenger(StudentEntity passenger)
        {
            var dbset = _context.Set<StudentInfo>();
            StudentInfo _passenger = new StudentInfo();
            _passenger.Id = passenger.Id;
            _passenger.FirstName = passenger.FirstName;
            _passenger.LastName = passenger.LastName;
            _passenger.Phone = passenger.Phone;
            _passenger.Email = passenger.Email;
            _passenger.Gender = passenger.Gender;
            _passenger.CountryId = passenger.CountryId;
            _passenger.StateId = passenger.StateId;
            dbset.Add(_passenger);
            _context.SaveChanges();
        }

        public StudentEntity GetPassengerById(int passengerId)
        {
            var passenger = base.Get(x => x.Id == passengerId);
            StudentEntity _passenger = null;
            if (passenger != null)
            {
                _passenger = new StudentEntity();
                _passenger.Id = passenger.Id;
                _passenger.FirstName = passenger.FirstName;
                _passenger.LastName = passenger.LastName;
                _passenger.Phone = passenger.Phone;
                _passenger.Email = passenger.Email;
                _passenger.Gender = passenger.Gender;
                _passenger.CountryId = passenger.CountryId ?? 0;
                _passenger.StateId = passenger.StateId ?? 0;
            }

            return _passenger;
        }

        public void DeletePassengerById(int passengerId)
        {
            var passengerData = base.GetAll(x => x.Id == passengerId).ToList();
            var dbset = _context.Set<StudentInfo>();
            dbset.RemoveRange(passengerData);
            _context.SaveChanges();
        }
    }
}
