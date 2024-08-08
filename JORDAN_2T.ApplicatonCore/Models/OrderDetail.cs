using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORDAN_2T.ApplicationCore.Models;

    public class OrderDetail
    {
        public int Id {  get; set; }
        [Required]
        public int OrderId {  get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order Order { get; set; }
        [Required]
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        [ValidateNever]
        public Movie Movie { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }

