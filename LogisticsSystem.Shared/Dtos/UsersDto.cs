using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Shared.Dtos
{
    public class UsersDto : BaseDto
    {
		private string account;

		public string Account
		{
			get { return account; }
			set { account = value; OnPropertyChanged(); IsModified = true; }
        }

		private string? name;
		public string? Name
		{
			get { return name; }
			set { name = value; OnPropertyChanged(); IsModified = true; }
		}

		private string password = "123";
		public string Password
		{
			get { return password; }
			set { password = value; OnPropertyChanged(); IsModified = true; }
        }

		private int level = 1;
		public int Level
        {
			get { return level; }
			set { level = value; OnPropertyChanged(); IsModified = true; }
        }

        private string? tag;
        public string? Tag
        {
            get { return tag; }
            set { tag = value; OnPropertyChanged(); IsModified = true; }
        }

        public bool IsModified { get; set; } = false;
        public bool IsNew { get; set; } = false;		
    }
}
