using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HSC.Dal.Configurations
{
    internal class AnalysisConfiguration: IEntityTypeConfiguration<Analysis>
    {
        public void Configure(EntityTypeBuilder<Analysis> builder)
        {
            builder.ToTable("Analysis");
        }
    }
}
