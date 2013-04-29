namespace SCRegistrationWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using SCRegistrationWeb.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SCRegistrationWeb.Models.SCRegistrationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SCRegistrationWeb.Models.SCRegistrationContext context)
        {
            context.Statuses.AddOrUpdate(
                 s => s.StatusID,
                 new Status { StatusID = 1, Name = "Incomplete 不完整" },
                 new Status { StatusID = 2, Name = "Pending 待完成" },
                 new Status { StatusID = 3, Name = "Confirmed 证实了" },
                 new Status { StatusID = 4, Name = "Cancelled 取消了" },
                 new Status { StatusID = 5, Name = "Checked In" },
                 new Status { StatusID = 6, Name = "Checked Out" }
                 );

            context.Services.AddOrUpdate(
                v => v.ServiceID,
                new Service { ServiceID = 1, Name = "English" },
                new Service { ServiceID = 2, Name = "Mandarin 普通话" },
                new Service { ServiceID = 3, Name = "Cantonese 广东话" },
                new Service { ServiceID = 4, Name = "T4C West 西区分堂" },
                new Service { ServiceID = 5, Name = "Youth Group" },
                new Service { ServiceID = 6, Name = "Children" },
                new Service { ServiceID = 7, Name = "Nursery" }
                );

            context.AgeRanges.AddOrUpdate(
                a => a.AgeRangeID,
                new AgeRange { AgeRangeID = 1, Name = "0-1 yrs" },
                new AgeRange { AgeRangeID = 2, Name = "1-5 yrs" },
                new AgeRange { AgeRangeID = 3, Name = "6-12 yrs" },
                new AgeRange { AgeRangeID = 4, Name = "Teenager" },
                new AgeRange { AgeRangeID = 5, Name = "Adult" },
                new AgeRange { AgeRangeID = 6, Name = "Senior" }
                );

            context.Genders.AddOrUpdate(
                 a => a.GenderID,
                 new Gender { GenderID = 1, Name = "Male 男" },
                 new Gender { GenderID = 2, Name = "Female 女" }
                 );

            context.Fellowships.AddOrUpdate(
                a => a.FellowshipID,
                new Fellowship { FellowshipID = 1, ServiceID = 1, Name = "No Fellowship Selected" },
                new Fellowship { FellowshipID = 2, ServiceID = 2, Name = "None Selected 没有选择" },
                new Fellowship { FellowshipID = 3, ServiceID = 3, Name = "None Selected 没有选择" },
                new Fellowship { FellowshipID = 4, ServiceID = 4, Name = "None Selected 没有选择" },
                new Fellowship { FellowshipID = 5, ServiceID = 5, Name = "No Fellowships" },
                new Fellowship { FellowshipID = 6, ServiceID = 6, Name = "No Fellowships" },
                new Fellowship { FellowshipID = 7, ServiceID = 7, Name = "No Fellowships" },
                new Fellowship { FellowshipID = 8, ServiceID = 1, Name = "Couples Group South" },
                new Fellowship { FellowshipID = 9, ServiceID = 1, Name = "College Group" },
                new Fellowship { FellowshipID = 10, ServiceID = 1, Name = "English District" },
                new Fellowship { FellowshipID = 11, ServiceID = 1, Name = "Plymouth Fellowship" },
                new Fellowship { FellowshipID = 12, ServiceID = 1, Name = "Young Profs/Grad Student" },
                new Fellowship { FellowshipID = 13, ServiceID = 1, Name = "Others" },
                new Fellowship { FellowshipID = 14, ServiceID = 2, Name = "Joy 喜樂" },
                new Fellowship { FellowshipID = 15, ServiceID = 2, Name = "Living Water 活水" },
                new Fellowship { FellowshipID = 16, ServiceID = 2, Name = "New Life 新生命" },
                new Fellowship { FellowshipID = 17, ServiceID = 2, Name = "West 西區" },
                new Fellowship { FellowshipID = 18, ServiceID = 2, Name = "Agape 仁愛" },
                new Fellowship { FellowshipID = 19, ServiceID = 2, Name = "Others 另外" },
                new Fellowship { FellowshipID = 20, ServiceID = 3, Name = "Cantonese Career 粵語就業" },
                new Fellowship { FellowshipID = 21, ServiceID = 3, Name = "Cantonese Student 粵語學生" },
                new Fellowship { FellowshipID = 22, ServiceID = 3, Name = "Chong Yuen 狀元" },
                new Fellowship { FellowshipID = 23, ServiceID = 3, Name = "Fung Sing 豐盛" },
                new Fellowship { FellowshipID = 24, ServiceID = 3, Name = "Grace 加恩" },
                new Fellowship { FellowshipID = 24, ServiceID = 3, Name = "Life 生命" },
                new Fellowship { FellowshipID = 24, ServiceID = 3, Name = "Song of Songs 雅歌" },
                new Fellowship { FellowshipID = 25, ServiceID = 3, Name = "Others 另外" },
                new Fellowship { FellowshipID = 26, ServiceID = 4, Name = "Albert He"},
                new Fellowship { FellowshipID = 27, ServiceID = 4, Name = "Longfei Hu" },
                new Fellowship { FellowshipID = 28, ServiceID = 4, Name = "Jie Gong" },
                new Fellowship { FellowshipID = 29, ServiceID = 4, Name = "Richard He" },
                new Fellowship { FellowshipID = 30, ServiceID = 4, Name = "Bingwen Hao" },
                new Fellowship { FellowshipID = 31, ServiceID = 4, Name = "Sherman Chan" }
                );

            context.RegTypes.AddOrUpdate(
                 a => a.RegTypeID,
                 new RegType { RegTypeID = 1, Name = "Full Time 全時間" },
                 new RegType { RegTypeID = 2, Name = "Part Time 部分時間" }
                 );

            context.RegPrices.AddOrUpdate(
                a => a.AgeRangeID,
                new RegPrice { AgeRangeID = 1, PartTimePrice = (decimal)10, FullTimePrice = (decimal)10},
                new RegPrice { AgeRangeID = 2, PartTimePrice = (decimal)25, FullTimePrice = (decimal)45},
                new RegPrice { AgeRangeID = 3, PartTimePrice = (decimal)40, FullTimePrice = (decimal)70},
                new RegPrice { AgeRangeID = 4, PartTimePrice = (decimal)55, FullTimePrice = (decimal)95},
                new RegPrice { AgeRangeID = 5, PartTimePrice = (decimal)55, FullTimePrice = (decimal)95},
                new RegPrice { AgeRangeID = 6, PartTimePrice = (decimal)55, FullTimePrice = (decimal)95}
                );

            context.RoomTypes.AddOrUpdate(
                a => a.RoomTypeID,
                new RoomType { RoomTypeID = 1, RegTypeID = 1, Name = "No Preference 没有偏好" },
                new RoomType { RoomTypeID = 2, RegTypeID = 2, Name = "No Room 无房必要" },
                new RoomType { RoomTypeID = 3, RegTypeID = 1, Name = "Family Room" },
                new RoomType { RoomTypeID = 4, RegTypeID = 1, Name = "Couple Room " },
                new RoomType { RoomTypeID = 5, RegTypeID = 1, Name = "Single-sex Dorm" },
                new RoomType { RoomTypeID = 6, RegTypeID = 1, Name = "Handicapped 残废" }
                );


            context.PmtStatuses.AddOrUpdate(
                a => a.PmtStatusID,
                new PmtStatus { PmtStatusID = 1, Name = "Pending" },
                new PmtStatus { PmtStatusID = 2, Name = "Approved" },
                new PmtStatus { PmtStatusID = 3, Name = "Declined" }
                );

            context.PmtTypes.AddOrUpdate(
                a => a.PmtTypeID,
                new PmtType { PmtTypeID = 1, Name = "Scholorship" },
                new PmtType { PmtTypeID = 2, Name = "Cash" },
                new PmtType { PmtTypeID = 3, Name = "Check" },
                new PmtType { PmtTypeID = 4, Name = "Refund" },
                new PmtType { PmtTypeID = 5, Name = "Adjustmet" },
                new PmtType { PmtTypeID = 6, Name = "Credit Card"}

                );



        }
    }
}
