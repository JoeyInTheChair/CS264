using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateVolleyballTeams
{
    class Player
    {
        private string name;
        private string position;
        private string rating;
        public void player(string name, string position, string rating)
        {
            this.name = name;
            this.position = position;
            if(rating.Equals("1")) { this.rating = "Main Team";  }
            else { this.rating = "beginner"; }
        }
        public string getName() {  return this.name; }  
        public string getPosition() { return this.position; }
        public string getRating() { return this.rating; }
    }
}
