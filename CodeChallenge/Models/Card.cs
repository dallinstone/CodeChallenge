using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class Card
    {
        public required string Suit {get; set;}
        public required string Val {get; set;}

        //just a bit of minmaxing to make the output look nice if we convert to string instead of returning 
        //      a JSON object as a string
        public override string ToString()
        {
            return $"{Val} of {Suit}";
        }
    }
}