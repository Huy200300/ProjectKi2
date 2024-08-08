using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JORDAN_2T.ApplicationCore.Models;

    public class Photo
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int Sequence { get; set; }
        public string LinkToSmallImage { get; set; }
        public string LinkToLargeImage { get; set; }
        public Movie Movie { get; set; }
        
    }

