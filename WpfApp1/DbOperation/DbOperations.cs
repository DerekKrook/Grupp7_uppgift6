﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using WpfApp1.DbOperation;
using WpfApp1.Models;
using Dapper;
using System.Data;
using WpfApp1.ErrorHandler;

namespace WpfApp1
{
    static class DbOperations
    {
        //Hämtar specifikt barn SÖK för- och efternamn.
        public static List<Child> GetChildren(string input)
        {
            InputHandler inputhandler = new InputHandler();
            var a = inputhandler.Uppercase(input);

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Child>($@"SELECT child.id, child.firstname, child.lastname, child.leavealone, class_id, class.name AS Class 
                FROM(child INNER JOIN class on class_id = class.id) 
                WHERE child.firstname LIKE '%{a}%' OR child.lastname LIKE '%{a}%';").ToList();

                return output;

            }

        }

        //Hämtar alla barn
        public static List<Child> GetAllChildren()
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Child>($@"SELECT child.id, child.firstname, child.lastname, child.leavealone, department.name AS Class 
                FROM((child INNER JOIN class ON class_id = class.id) 
                INNER JOIN department ON department_id = department.id) 
                ORDER BY department.name DESC").ToList();

                return output;
            }
        }
        // hämtar årkurs 1
        public static List<Child> GetFirstGraders()
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Child>($@"SELECT child.id, child.firstname, child.lastname, child.leavealone, class.name AS Class, department.id AS avdelning 
                FROM((child INNER JOIN class ON class_id = class.id) 
                INNER JOIN department ON department_id = department.id) 
                WHERE department_id = 1 ORDER BY class_id DESC;").ToList();


                return output;
            }
        }
        // hämtar årkurs 2
        public static List<Child> GetSecondGraders()
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Child>($@"SELECT child.id, child.firstname, child.lastname, child.leavealone, class.name AS Class, department.id AS avdelning 
                FROM((child INNER JOIN class ON class_id = class.id) INNER JOIN department ON department_id = department.id) 
                WHERE department_id = 2 ORDER BY class_id DESC;").ToList();

                return output;
            }
        }
        // hämtar årkurs 3
        public static List<Child> GetThirdGraders()
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Child>($@"SELECT child.id, child.firstname, child.lastname, child.leavealone, class.name AS Class, department.id AS avdelning 
                FROM((child INNER JOIN class ON class_id = class.id) 
                INNER JOIN department ON department_id = department.id)
                WHERE department_id = 4 ORDER BY class_id DESC;").ToList();

                return output;
            }
        }

        //Hämtar alla anställda
        public static List<Staff> GetAllStaff()
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Staff>($@"SELECT * FROM staff").ToList();

                return output;
            }
        }

        //Hämtar alla föräldrar
        public static List<Guardian> GetAllGuardians()
         {

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {              
                var output = connection.Query<Guardian>($@"SELECT * FROM guardian").ToList();

                return output;
            }
        }

        //Hämtar föräldrar efter sökning av för - och efternamn
        public static List<Guardian> GetGuardian(string firstName, string lastName)
        {
            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Guardian>($@"SELECT * FROM child 
                WHERE firstname LIKE '%{firstName}%' OR lastname LIKE '%{lastName}%'").ToList();

                return output;
            }
        }

        //Hämtar barn till vårdnadshavare 
        public static List<Child> GetChildrenOfGuardian()
        {
            var Id = Activeguardian.Id;

            var Query = $@"SELECT * FROM guardian_child INNER JOIN child ON child_id = child.id WHERE guardian_id='{Id}'"; 

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Child>(Query).ToList();

                return output;
            }
        }

        // Hämtar vårdnadshavare till barn
        public static List<Guardian> GetGuardianOfChild(Child child)
        {

            var Id = child.Id;
                      
            var Query = $@"SELECT * FROM guardian_child 
            INNER JOIN guardian ON guardian_id = guardian.id 
            WHERE child_id='{Id}'";

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Guardian>(Query).ToList();

                return output;
            }
        }

        //Hämta scheman för barn   Vill ha med category attendance ID  för dag/barnet 
        public static List<Schedule> GetSchedule (int Id, string day)
        {

            Schedule s = new Schedule();
            List <Schedule> schedules = new List<Schedule>();

            var Query = $@"SELECT lecture.name AS Lecturename, dates.day AS Day, time.timestart AS Timestart, time.timefinish AS Timefinish 
            FROM ((((((child INNER JOIN schedule ON child.id = child_id) 
            INNER JOIN schedule_lecture ON schedule.id = schedule_id) 
            INNER JOIN lecture ON lecture__id = lecture.id) 
            INNER JOIN lecture_dates_time ON lecture.id = lecture_id) 
            INNER JOIN dates ON dates_id = dates.id) INNER JOIN time ON time_id = time.id)
            WHERE child.id='{Id}' AND dates.day='{day}' ORDER BY time.timestart ASC";           

            using (var conn = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(Query, conn))
                    using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        s = new Schedule()

                        {
                            Lecturename = reader["Lecturename"].ToString(),
                            Day = reader["Day"].ToString(),
                            Timestart = Convert.ToDateTime((reader["Timestart"]).ToString()),
                            Timefinish = Convert.ToDateTime((reader["Timefinish"]).ToString())

                        };

                    schedules.Add(s);
                    s.changeDateTime();
                    }
                }
            }
            return schedules;
        }

        //Hämtar barn på fritids
        public static List<Attendance> GetChildrenAtFritids()
        {

            Attendance a = new Attendance();
            List<Attendance> attendance = new List<Attendance>();

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Attendance>($@"SELECT attendance_id AS id, child.firstname ||' '|| child.lastname AS Child, guardian.firstname ||' '|| guardian.lastname AS Guardian, category_attendance.name_type AS Category_attendance, dates.day AS Day, attendance.comment AS Comment, child.leavealone AS LeaveAlone
            FROM (((((attendance INNER JOIN child ON child_id = child.id) INNER JOIN guardian ON guardian_id = guardian.id) 
            INNER JOIN category_attendance ON category_attendance_id = category_attendance.id)  
            INNER JOIN attendance_dates ON attendance_dates.dates_id = attendance_dates.dates_id AND attendance.id = attendance_dates.attendance_id) 
            INNER JOIN dates ON attendance_dates.dates_id = dates.id AND dates.day = dates.day) 
            WHERE category_attendance_id = 3 
            ORDER BY dates.day;").ToList();

                return output;
            }

        }


        // uppdatera mail och/eller telefon på förälder EJ KLAR
        public static List<Guardian> UpdateGuardianProperties(int phone, string email, string firstname, string lastname)
        {
            var Id = Activeguardian.Id;
            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Guardian>($@"UPDATE guardian SET firstname = '{firstname}', lastname = '{lastname}', email = '{email}', phone = {phone} WHERE id = {Id}").ToList();

                return output;
            }

        }
        public static List<Guardian> AddNewGuardian(int phone, string firstname, string lastname, string email)
        {

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Guardian>($@"INSERT INTO guardian (firstname, lastname, phone, email) VALUES ('{firstname}', '{lastname}', {phone}, '{email}')").ToList();


                return output;
            }

        }
        public static List<Child> UpdateChildProperties(string firstname, string lastname)
        {
            var Id = Activechild.Id;

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Child>($@"UPDATE child SET firstname = '{firstname}', lastname = '{lastname}' WHERE id = {Id}").ToList();

                return output;
            }

        }
  

        public static List<Child> AddNewChild(string firstname, string lastname)
        {

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Child>($@"INSERT INTO child (firstname, lastname, leavealone) VALUES ('{firstname}', '{lastname}')").ToList();
                
                return output;
            }

        }
        public static List<Child> DeleteChild()
        {
            var Id = Activechild.Id;

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Child>($@"DELETE FROM child WHERE id = {Id};").ToList();


                return output;
            }

        }



       
        public static List<Connections> GetChildGuardian()
        {

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                //LÄGG TILL I TABELL GUARDIAN_CHILD
                var output = connection.Query<Connections>
                    ($@"SELECT guardian_id,  guardian.firstname ||' '|| guardian.lastname AS Guardian, child_id, child.firstname ||' '|| child.lastname AS Child
                    FROM((guardian_child INNER JOIN child ON child_id = child.id AND child.firstname = child.firstname AND child.lastname = child.lastname) 
                    INNER JOIN guardian ON guardian_id = guardian.id AND guardian.firstname = guardian.firstname AND guardian.lastname = guardian.lastname);").ToList();

          

                return output;
            }

}
        public static List<Guardian> ConnectChildAndGuardian()
        {

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
               
                var output = connection.Query<Guardian>($@"INSERT INTO guardian_child(guardian_id, child_id) VALUES('{Activeguardian.Id}', '{Activechild.Id}')").ToList();
  

        return output;
    }

}
        // EJ KLAR
    //    public static List<Guardian> DeleteConnection()
    //    {

    //        using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
    //        {
               
    //            var output = connection.Query<Guardian>($@"DELETE FROM guardian_child WHERE guardian_id = {Activeguardian.Id} AND child_id = {Activechild.Id}").ToList();
  

    //    return output;
    //}
//}


       public static List<Guardian> DeleteGuardian()
        {
            var Id = Activeguardian.Id;

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Guardian>($@"DELETE FROM guardian WHERE id = {Id};").ToList();


                return output;
            }

        }

        // Sätt gå hem EJ KLAR 
        public static List<Attendance> GoneHome(Attendance home, int id)
        {

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Attendance>($@"UPDATE attendance SET gonehome = {home}, WHERE id = { id }").ToList();

                return output;
            }

        }

        //Hämtar Avdelning och Telefonnummer 
        public static List<Department> ContactDepartment()
        {
            List<Department> departments = new List<Department>();

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {

                var output = connection.Query<Department>($@"SELECT * FROM Department").ToList();

                return output;
            }
        }

        //Lägg till frånvaro EJ KLAR Behövs lägga till datum!
        public static List<Attendance> GuardianReportAttendance(int id, string comment, string day)
        {

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Attendance>($@"INSERT INTO attendance (id, guardian_id, child_id, comment, category_attendance_id) VALUES ('{id}', '{Activeguardian.Id}', '{Activechild.Id}', '{comment}', '{ActiveAttendancecategory.Id}')").ToList();


                return output;
            }

        }

        //Hämta category attendances
        public static List<Attendancecategory> GetAttendances()
        {

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Attendancecategory>($@"SELECT * FROM category_attendance WHERE present = false").ToList();


                return output;
            }

        }

        //Hämtar alla datum
        public static List<Date> GetDays()
        {

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Date>($@"SELECT * FROM dates").ToList();


                return output;
            }

        }

        //Hämtar alla Veckor
        public static List<Weeks> GetWeek()
        {

            using (IDbConnection connection = new NpgsqlConnection(ConnString.ConnVal("dbConn")))
            {
                var output = connection.Query<Weeks>($@"SELECT week FROM dates GROUP BY week").ToList();


                return output;
            }

        }
    }
}

