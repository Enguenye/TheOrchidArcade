using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheOrchidArchade.Context;
using TheOrchidArchade.Models;

namespace TheOrchidArchade.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;


        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }


    }
}
