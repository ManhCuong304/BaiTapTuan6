using lab6_DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab6_BUS
{
    public class StudentService
    {
        public List<Student> GetAll()
        {
            Model1 context = new Model1();
            return context.Students.ToList();
        }
        public List<Student> GetAllHasNoMajor()
        {
            Model1 context = new Model1();
            return context.Students.Where(p => p.MajorID == null).ToList();
        }

        public List<Student> GetAllHasNoMinor(int facultyID)
        {
            Model1 context = new Model1();
            return context.Students.Where(p => p.MajorID == null && p.FacultyID == facultyID).ToList();

        }

        public Student FinById(int studentId)
        {
            Model1 context = new Model1();
            return context.Students.FirstOrDefault(p => p.StudentID == studentId);
        }
        public void InsertUpdate(Student s)
        {
            Model1 context = new Model1();
            context.Students.AddOrUpdate(s);
            context.SaveChanges();
        }
        public void InsertOrUpdateStudent(Student student)
        {
            if (string.IsNullOrEmpty(student.FullName))
            {
                throw new Exception("Tên sinh viên không được để trống.");
            }

            using (Model1 context = new Model1())
            {
                context.Students.AddOrUpdate(student);
                context.SaveChanges();
            }
        }
        public void DeleteStudentById(int studentId)
        {
            using (Model1 context = new Model1())
            {
                var student = context.Students.FirstOrDefault(p => p.StudentID == studentId);
                if (student != null)
                {
                    context.Students.Remove(student);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Không tìm thấy sinh viên.");
                }
            }
        }
        public void DeleteStudent(int studentId)
        {
            using (Model1 context = new Model1())
            {
                var studentToRemove = context.Students.Find(studentId);
                if (studentToRemove != null)
                {
                    context.Students.Remove(studentToRemove);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Sinh viên không tồn tại.");
                }
            }
        }
    }
}
