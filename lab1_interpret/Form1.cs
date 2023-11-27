using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba1
{
    
    public partial class Form1 : Form
    {
        Rules[] rules = new Rules[40];
        StreamReader sr = null;
        String filePath = @"C:\code.txt";
        String str="";
        List<int> error_spisok = new List<int>();
        List<String> output = new List<String>();
        
        Hashtable peremennaya = new Hashtable();

        public Form1()
        {
            InitializeComponent();
            for(int i=0;i<40;i++)
            {
                rules[i] = new Rules();
            }
            rules[1].reg = new Regex(@"^\.$");
            rules[2].reg = new Regex(@"^;$");
            rules[3].reg = new Regex(@"^program$");
            rules[4].reg = new Regex(@"^begin$");
            rules[5].reg = new Regex(@"^end$");
            rules[6].reg = new Regex(@"^[0-3]+$");
            rules[7].reg = new Regex(@"^$");
            rules[8].reg = new Regex(@"^$");
            rules[9].reg = new Regex(@"^\($");
            rules[10].reg = new Regex(@"^\)$");
            rules[11].reg = new Regex(@"^'$");
            rules[12].reg = new Regex(@"^var$");
            rules[13].reg = new Regex(@"^[A-Za-z]+[A-Za-z0-3]*$");
            rules[14].reg = new Regex(@"^:$");
            rules[15].reg = new Regex(@"^integer$");
            rules[16].reg = new Regex(@"^,$");
            rules[17].reg = new Regex(@"^=$");
            rules[18].reg = new Regex(@"^while$");
            rules[19].reg = new Regex(@"^do$");
            rules[20].reg = new Regex(@"^writeln$");
            rules[21].reg = new Regex(@"^<$");
            rules[22].reg = new Regex(@"^>$");
            rules[23].reg = new Regex(@"^\+$");
            rules[24].reg = new Regex(@"^-$");
            rules[25].reg = new Regex(@"^\*$");
            rules[26].reg = new Regex(@"^/$");
            rules[27].reg = new Regex(@"^$");
            rules[28].reg = new Regex(@"^$");
            rules[29].reg = new Regex(@"^$");
            rules[30].reg = new Regex(@"^$");
                    
        }
        int count = 0;
        String next_() // передает следующий элемент кода
        {
            String line = sr.ReadLine(); 
            count++;
            Regex reg = new Regex(@"\s");
            line = reg.Replace(line, "");
            return line;
        }
        int program()
        {
            bool prog = false, nazv = false, endline = false, error = false;
            while ((!prog || !nazv || !endline) && str != null && !error) // 
            {
                if (!prog)
                    if (rules[3].reg.IsMatch(str))
                    {
                        prog = true;
                    }
                    else
                    {
                        error = true;
                    }
                else if (!nazv)
                    if (rules[13].reg.IsMatch(str))
                    {
                        nazv = true;
                    }
                    else
                    {
                        error = true;
                    }
                else if (!endline)
                    if (rules[2].reg.IsMatch(str))
                    {
                        endline = true;
                    }
                    else
                    {
                        error = true;
                    }
                str = next_();
            }
            if (error)
            {
                if (!prog)
                    return 9;
                else if (!nazv)
                    return 10;
                else if (!endline)
                    return 11;
                else
                    return 12;
            }
            else
                return 0;
        }
        int spisok_peremrnnyx()
        {
            bool perem = false, error = false;
            //while ((!perem||!zpt) && str != null && !error) // -----------------------------------тут надо другой выход из цикла
            while (!rules[14].reg.IsMatch(str) && str != null && !error) // ждем : и потом в spisok_identificatorov пропускаем 1 чтение 
            {
                if (!perem)
                    if (rules[13].reg.IsMatch(str))
                    {
                        peremennaya.Add(str, 0);
                        perem = true;
                    }
                    else
                    {
                        error = true;
                    }
                else if (perem)
                    if (rules[16].reg.IsMatch(str))
                    {
                        perem = false;
                    }
                    else
                    {
                        error = true;
                    }
                else
                    error = true;
                str = next_();
            }
            if (!perem)
                error = true;
            if(error)
            {
                if (!perem)
                    return 1; // ожидалась переменная
                else
                    return 2; // недопустимое имя переменной
            }
            else
                return 0;
        }
        int spisok_identificatorov()
        {
            bool var = false, spis = false, dvtoch = false, tip = false, endline = false, error = false;
            int last_error = 0;
            bool dont_read = false;
            while ((!var||!spis||!dvtoch||!tip||!endline) && str != null && !error) 
            {
                if (!var)
                    if (rules[12].reg.IsMatch(str))
                    {
                        var = true;
                    }
                    else
                    {
                        error = true;
                    }
                else if (!spis)
                {
                    last_error = spisok_peremrnnyx();
                    if (last_error == 0)
                    {
                        spis = true;
                        dont_read = true;
                    }
                    else
                    {
                        error = true;
                        error_spisok.Add(last_error);
                    }
                }
                else if (!dvtoch)
                    if (rules[14].reg.IsMatch(str))
                    {
                        dvtoch = true;
                    }
                    else
                    {
                        error = true;
                    }
                else if (!tip)
                    if (rules[15].reg.IsMatch(str))
                    {
                        tip = true;
                    }
                    else
                    {
                        error = true;
                    }
                else if (!endline)
                    if (rules[2].reg.IsMatch(str))
                    {
                        endline = true;
                    }
                    else
                    {
                        error = true;
                    }
                else 
                    error = true;
                if(!dont_read)
                    str = next_();
                else
                {
                    dont_read = false;
                }
            }
            if (error)
            {
                if (!var)
                    return 3;
                else if (!spis)
                    return 4;
                else if (!dvtoch)
                    return 5;
                else if (!tip)
                    return 6;
                else if (!endline)
                    return 7;
                else
                    return 8;
            }
            else
                return 0;
        }

        int check_operation(bool delat=true) // только операции без begin и end;
        {
            String p;
            int val;
               
            if(rules[18].reg.IsMatch(str)) // case
            {
                str = next_();
                //
                if (peremennaya.ContainsKey(str)) // переменная
                {
                    p = str;
                    str = next_();
                    if (rules[19].reg.IsMatch(str)) // of
                    {
                        str = next_();
                        int last_error = spisok_vibora(p);

                        if (last_error == 1014) // список выбора
                        {
                            //str = next_(); не читаем тк читали в списке выбора
                            if (rules[5].reg.IsMatch(str)) // end
                            {
                                str = next_();
                                if (rules[2].reg.IsMatch(str)) // ;
                                {
                                    return 1007;
                                }
                                else return 1012;
                            }
                            else return 1011;
                        }
                        else
                        {
                            error_spisok.Add(last_error);
                            return 1010;
                        }
                            
                    }
                    else return 1009;
                }
                else return 1008;
            }
            else if(rules[20].reg.IsMatch(str)) // writeln
            {
                str=next_();
                if (rules[9].reg.IsMatch(str)) // (
                {
                    str = next_();
                    if (rules[11].reg.IsMatch(str)) // '
                    { 
                        str = next_();
                        // строка 
                        p=str;
                        str = next_();
                        if (rules[11].reg.IsMatch(str)) // '
                        {
                            str= next_();
                            if (rules[10].reg.IsMatch(str)) // )
                            {
                                str = next_();
                                if (rules[2].reg.IsMatch(str)) // ;
                                {
                                    if(delat)
                                        output.Add(p + "\n");
                                    return 1020;
                                }
                                else return 1024;
                            }
                            else return 1022;
                        }
                        else return 1023;
                    }
                    else return 1023;
                }
                else return 1021;
            }
            else if (peremennaya.ContainsKey(str)) // переменная
            {
                p = str;
                str = next_();
                if (rules[14].reg.IsMatch(str)) // :
                {
                    str = next_();
                    if (rules[17].reg.IsMatch(str)) // =
                    {
                        str = next_();
                        if (rules[6].reg.IsMatch(str)) // число
                        {
                            // надо распознать число
                            val = Int32.Parse(str);
                            str = next_();
                            if (rules[2].reg.IsMatch(str))
                            {
                                if(delat)
                                    peremennaya[p] = val;
                                return 1000;
                            }
                            else return 1005;
                        }
                        else return 1001;
                    }
                    else return 1003;
                }
                else return 1002;
            }
            else return 1027;  
        }

        int spisok_vibora(String p)
        {
            int val;
            bool select = false;
            bool chislo = false, dvtoch = false, commands = false, error = false;
            int last_error = 0;
            bool dont_read = false;
            while (!rules[5].reg.IsMatch(str) && str != null && !error) // тут ждем end  тк конец case это end
            {
                if (chislo && dvtoch && commands)// значит этот список выбора закончился
                {
                    chislo = false;
                    dvtoch = false;
                    commands = false;
                    last_error = 0;
                    select = false;
                }

                if (!chislo)
                    if (rules[6].reg.IsMatch(str)) // число
                    {
                        val=Int32.Parse(str);
                        if ((int)peremennaya[p] == val)
                            select = true;
                        chislo = true;
                    }
                    else
                    {
                        error = true;
                        return 1015;
                    }
                else if (!dvtoch)
                    if (rules[14].reg.IsMatch(str)) // :
                    {
                        dvtoch = true;
                    }
                    else
                    {
                        error = true;
                        return 1016;
                    }
                else if (!commands)
                {
                    while (last_error!=1027 && str != null && !error)// здесь проверяем все операции
                    {
                        last_error = check_operation(select);
                        if (last_error == 1000) // присвоение
                        {
                            last_error = 1028;
                        }
                        else if (last_error == 1007) // case
                        {
                            last_error = 1028;
                        }
                        else if (last_error == 1020) // writeln
                        {
                            last_error = 1028;
                        }
                        else if (last_error == 1027) // конец операторов
                        {

                        }
                        else 
                        {
                            error = true;
                            return last_error;
                        }
                        if(last_error != 1027)
                            str = next_();
                    }
                    commands = true;
                    dont_read = true;

                }

                if (!dont_read)
                    str = next_();
                else
                {
                    dont_read = false;
                }
                
            }
            return 1014;
        }

        int spisok_operatorov()
        {
            bool error = false, end = false, begin = false, endline = false, commands = false;
            int last_error = 0;
            bool dont_read = false;
            while((!begin||!end||!endline&&!commands) && str != null && !error)
            {
                if (!begin)
                    if (rules[4].reg.IsMatch(str))
                    {
                        begin = true;
                    }
                    else
                    {
                        error = true;
                    }
                else if(!commands)
                {
                    while (!rules[5].reg.IsMatch(str) && str != null && !error)// здесь проверяем все операции
                    {
                        last_error=check_operation();
                        if (last_error == 1000) // присвоение
                        {

                        }
                        else if(last_error == 1007) // case
                        {

                        }
                        else
                        {
                            error = true;
                            return last_error;
                        }
                        
                        str = next_();
                    }
                    commands = true;
                    dont_read = true;
                }  
                else if (!end)
                    if (rules[5].reg.IsMatch(str))
                    {
                        end = true;
                    }
                    else
                    {
                        error = true;
                    }
                else if (!endline)
                    if (rules[1].reg.IsMatch(str))
                    {
                        endline = true;
                    }
                    else
                    {
                        error = true;
                    }
                if (!dont_read)
                    str = next_();
                else
                {
                    dont_read = false;
                }
            }
            if(error)
            {
                return last_error;
            }
            else
                return 0;
        }
        int osnova()
        {
            bool prog = false,spis_id = false,spis_oper = false,error=false;
            int last_error = 0;
            while ((!prog||!spis_id||!spis_oper) && str != null && !error)
            {
                if (!prog)
                {
                    last_error = program();
                    if (last_error == 0)
                    {
                        prog = true;
                    }
                    else
                    {
                        error_spisok.Add(last_error);
                        error = true;
                    }
                }                 
                else if (!spis_id)
                {
                    last_error = spisok_identificatorov();
                    if (last_error == 0)
                    {
                        spis_id = true;
                    }
                    else
                    {
                        error_spisok.Add(last_error);
                        error = true;
                    }
                }
                else if (!spis_oper)
                {
                    last_error = spisok_operatorov();
                    if (last_error == 0)
                    {
                        spis_oper = true;
                    }
                    else
                    { 
                        error_spisok.Add(last_error);
                        error = true;
                    }
                }

            }
            if(error)
            {
                if (!prog)
                    return 100;
                else if (!spis_id)
                    return 101;
                else
                    return 102;
            }
            else
                return 0;
            }



        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            sr = new StreamReader(filePath);
            str = sr.ReadLine();
            if (str != null)
                if (osnova() == 0)
                {
                    str = "ok\n";
                    str += "vivod : ";
                    for (int i = 0; i < output.Count; i++)
                    {
                        str += output[i].ToString() + " ";
                    }
                }
                else
                {
                    str = "neok\n";
                    str += "error : ";
                    for (int i = 0; i < error_spisok.Count; i++)
                    {
                        str += error_spisok[i].ToString() + " ";
                    }
                }
            sr.Close();
            str += "\nperem : \n";
            var ky = peremennaya.Keys;
            foreach (string s in ky)
            {
                str += "\t" + s + " := " + peremennaya[s] + "\n";
            }
            MessageBox.Show(str);
            this.Close();
        }
    }

    internal class Rules
    {
        public Regex reg { get; internal set; }
    }
}
