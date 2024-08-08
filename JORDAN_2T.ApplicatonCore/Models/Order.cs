using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JORDAN_2T.ApplicationCore.Models;
public enum OrderStatus
    {
        Pending,
        Approved,
        Processing,
        Shipped,
        Cancelled,
        Refunded

    }
    public enum PaymentStatus{
        Pending,
        Approved,
        ApprovedForDelayedPayment,
        Rejected
    }
    public class Order
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        public decimal? OrderTotal { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
         public DateTime PaymentDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }

