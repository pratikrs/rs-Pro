using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOWFirstDemo.Entities
{
    public partial class StudentModel
    {
        public StudentModel()
        {
            this.AddressInfoes = new HashSet<AddressModel>();
            this.ContactInfoes = new HashSet<ContactModel>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> StateId { get; set; }

        public virtual ICollection<AddressModel> AddressInfoes { get; set; }
        public virtual ICollection<ContactModel> ContactInfoes { get; set; }
    }
}
