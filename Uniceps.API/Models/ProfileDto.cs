using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;

namespace Uniceps.API.Models
{
    public class ProfileDto
    {

        public string? uid { get; set; }
        public string? full_name { get; set; }
        public string? phone { get; set; }
        public int birth_date { get; set; }
        public bool gender_male { get; set; }
        //public void FromProfile(Profile profile)
        //{

        //    uid = profile.UID;
        //    full_name = profile.FullName;
        //    phone = profile.Phone;
        //    birth_date = profile.BirthDate;
        //    gender_male = profile.GenderMale;

        //}
        public Profile ToProfile()
        {
            Profile profile = new Profile()
            {
                UID = uid,
                FullName = full_name,
                Phone = phone,
                BirthDate = birth_date,
                GenderMale = Convert.ToBoolean(gender_male),
            };

            return profile;
        }
    }
}
