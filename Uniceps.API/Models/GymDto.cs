using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;

namespace Uniceps.API.Models
{
    public class GymDto
    {
        public string? id { get; set; }
        public string? gym_name { get; set; }
        public string? owner_name { get; set; }
        public string? phone_number { get; set; }
        public string? telephone { get; set; }
        public string? logo { get; set; }
        public string? address { get; set; }

        internal GymProfile ToGymProfile()
        {
            GymProfile gymProfile = new GymProfile()
            {
                GymId = id,
                OwnerName = owner_name,
                PhoneNumber = phone_number,
                Telephone = telephone,
                Logo = logo,
                Address = address,
                GymName = gym_name,
            };
            return gymProfile;
        }
        internal void FromGymProfile(GymProfile entity)
        {
            id = entity.GymId;
            owner_name = entity.OwnerName;
            phone_number = entity.PhoneNumber;
            telephone = entity.Telephone;
            logo = entity.Logo;
            address = entity.Address;
            gym_name = entity.GymName;

        }
    }
}
