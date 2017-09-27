using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BachelorModelViewController.Models
{
    public class ChannelContext : DbContext
    {
        public ChannelContext(DbContextOptions<ChannelContext> options) : base(options)
        {
        }

        public DbSet<Channel> Channels { get; set; }
    }
}
