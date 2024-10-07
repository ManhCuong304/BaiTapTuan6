using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using lab6_DAL;
using lab6_BUS;

namespace Lab6
{
    public partial class Form1 : Form
    {
        private readonly StudentService studentService=new StudentService();
        private readonly FacultiService facultiService=new FacultiService();


        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //List<Student>  students = studentService.GetAll();
            //dataGridView1.DataSource = students;
            lbl1.ForeColor = Color.Red;
            this.Text = "Quản Lý Sinh Viên";
            try
            {
                var listFacultys = facultiService.GetAll();
                var listStudents = studentService.GetAll();
                FillFacultyCombobox(listFacultys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        private void  FillFacultyCombobox(List<Faculty> listFacultys)
        {
            listFacultys.Insert(0, new Faculty());
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName" ;
            this.cmbFaculty.ValueMember = "FacultyID" ;

        }

        private void BindGrid (List<Student> listStudents)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.StudentID;
                dataGridView1.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                {
                    dataGridView1.Rows[index].Cells[3].Value = item.Faculty.FacultyName;
                    dataGridView1.Rows[index].Cells[2].Value = item.AverageScore + "";
                }
                if(item.MajorID!= null)
                {
                    dataGridView1.Rows[index].Cells[4].Value = item.Major.Name + "";
                }
            }


        }

        private void btnThemXoa_Click(object sender, EventArgs e)
        {
            try
            {
                Student student = new Student()
                {
                    StudentID = Convert.ToInt32(txtmsv.Text),
                    FullName = txtten.Text,
                    AverageScore = decimal.TryParse(txtdiem.Text, out decimal score) ? score : 0,
                    FacultyID = Convert.ToInt32(cmbFaculty.SelectedValue),
                };
                studentService.InsertOrUpdateStudent(student);
                MessageBox.Show("Lưu sinh viên thành công!");
                BindGrid(studentService.GetAll());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtmsv.Text = row.Cells[0].Value.ToString();
                txtten.Text = row.Cells[1].Value.ToString();
                txtdiem.Text = row.Cells[2].Value.ToString();
                cmbFaculty.SelectedValue = row.Cells[3].Value;
            }
        }

        private void Xoabtn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int studentId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?", "Xác nhận xóa", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        studentService.DeleteStudent(studentId);

                        MessageBox.Show("Xóa sinh viên thành công!");
                        BindGrid(studentService.GetAll());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }

        private void cbdk_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                List<Student> listStudents;

                if (cbdk.Checked)
                {
                    listStudents = studentService.GetAllHasNoMajor();
                }
                else
                {
                    listStudents = studentService.GetAll();
                }

                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
