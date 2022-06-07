using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
/*compatibilidad de tipos
 warnisn

 Primero la declaracion de variables
falta validar todos los caso posibles y los posisbles tokes que los reconosca
Modificar el codigo made rpara que este permita
 */

namespace Compilador
{
    public partial class Form1 : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        bool tipo = false;
        //Para almacenar los nombres de las variables
        List<string> L_enteros= new List<string>();
        List<string> L_float = new List<string>();
        List<string> L_string = new List<string>();
        List<string> L_bool = new List<string>();
        List<string> L_char = new List<string>();
        List<string> L_dobles = new List<string>();
        List<string> L_general = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btn_aceptar_Click(object sender, EventArgs e)
        {
         

            primera_funcion();
            //llamamos 
            validar();
            validar_main();//esta correcta
            txb_errores.Clear();
        }
        private void variables_desconocidas()
        {
            //cuando el progrma no indentifica una veriable entonces pasara a marcarse como error a la lista
            foreach (DataGridViewRow Row in data_v.Rows)
            {
                if (Convert.ToString(Row.Cells["Estado"].Value) == "desconocido")
                {
                    txb_errores.Text = txb_errores.Text + "\n comando desconocido en la fila "+Convert.ToString(Row.Cells["Fila"].Value);
                }
            }
        }//completado
        private void validar_variables(List<string> var)//las varibales desconocidas en la ejecucion son reconociodas
        {
            int numero = var.Count();
            int cont = 0;
            foreach (DataGridViewRow Row in data_v.Rows)
            {
                //estan todod los valores asignados
                int x = 0;
                bool enco = false;
                string estado = Convert.ToString(Row.Cells["Estado"].Value);
                string variable = Convert.ToString(Row.Cells["Variable"].Value);

                if (estado == "desconocido")
                {
                    for (x = 0; x < numero; x++)
                    {
                        if (variable == var[x])
                        {
                            enco = true;
                        }
                    }
                    if (enco == true)
                    {
                        //Correcion en las varibales desconocidas
                        data_v.Rows[cont].Cells[0].Value = "Variables";
                    }
                }
                cont++;
            }
        }//completado
        private void formato_Correcto_variables(List<string> values, int posicion)
        {
            //Checar que la declaracion de las variables sean correctas
            int cont = values.Count();
            //MessageBox.Show(cont.ToString());
            if (values[0] == "entero" || values[0] == "numero flotante" ||
                    values[0] == "valor booleano" || values[0] == "cadena de caracteres" || values[0] == "letra"
                    || values[0] == "numero extendido")
                
            {
                if (cont == 3)
                {
                    if (values[1] != "Variables")
                    {
                        // para la declaracion con asignacion
                        txb_errores.Text = txb_errores.Text + "\n Error en la declaracion de variables linea  " + posicion;
                    }
                }
                else
                {
                    if (cont == 5)
                    {
                        //Para la declaracion con asignacion
                        if (values[1] != "Variables" && values[2] != "igual")
                        {
                            txb_errores.Text = txb_errores.Text + "\n Error en la declaracion de variables linea  " + posicion;
                        }
                    }
                    else
                    {
                        txb_errores.Text = txb_errores.Text + "\n Error en la declaracion esta incompleta " + posicion;
                    }
                }
            }
         
        }//incompleto
        private void validar_main()
        {
         //aqui se validad que se inicio el programa con la palabra reservada main y las llaves  asi como el return 0
            try
            {
                int cont = data_v.RowCount;// Para sacar el numero de filas en numero

                if (Convert.ToString(data_v.Rows[0].Cells[1].Value) != "main")
                {
                    txb_errores.Text = txb_errores.Text + "\n No existe un main";
                }
                if (Convert.ToString(data_v.Rows[1].Cells[1].Value) != "(" || Convert.ToString(data_v.Rows[2].Cells[1].Value) != ")")
                {
                    txb_errores.Text = txb_errores.Text + "\n Faltan los parentesis del main";
                }
                if (Convert.ToString(data_v.Rows[3].Cells[1].Value) != "{")
                {
                    txb_errores.Text = txb_errores.Text + "\n No existe llave de entrada al curepo principal de la funcion";
                }
                if (Convert.ToString(data_v.Rows[cont - 2].Cells[1].Value) != "}")
                {
                    txb_errores.Text = txb_errores.Text + "\n ";
                }
                if (Convert.ToString(data_v.Rows[cont - 5].Cells[1].Value) != "return" && Convert.ToString(data_v.Rows[cont - 4].Cells[1].Value) != "0"
                    && Convert.ToString(data_v.Rows[cont - 5].Cells[1].Value) != ";")
                {
                    txb_errores.Text = txb_errores.Text + "\n El programa no tiene salida cero";
                }
            }
            catch
            {
                txb_errores.Text = txb_errores.Text + "\n Multiples errores en el programa";
            }
        }//completado
        private void correcion_float()
        {
            int cont = 0;
            
            foreach (DataGridViewRow Row in data_v.Rows)
            {
                //estan todod los valores asignados
                string estado = Convert.ToString(Row.Cells["Estado"].Value);
                if(estado=="numero flotante")
                {
                  

                    if(data_v.Rows[cont + 3].Cells[0].Value.ToString() == "desconocido")
                    {
                        try
                        {
                            float h= float.Parse(data_v.Rows[cont + 3].Cells[2].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture);
                            //MessageBox.Show(h.ToString());
                            data_v.Rows[cont+3].Cells[0].Value = "numero decimal";
                            break;
                        }

                        catch
                        {

                        }
                    }
                }
                cont++;
            }
                
          
        }//completado
        private void validar_variables_repetidas()
        {
            int count = L_general.Count();
            int rep = 0;
            for (int x =0; x < count; x++)
            {
                for (int y=0; y < count; y++)
                {
                    //MessageBox.Show("Vx    "+L_general[x]);
                   // MessageBox.Show("Vy     "+L_general[y]);
                    if (L_general[x] == L_general[y])
                    {
                        rep++;
                    }   
                }
                if (rep != 1)
                {
                    txb_errores.Text = txb_errores.Text + "\n La varieble " + L_general[x] + " se encuntra declarada multiples veces";
                }
                rep = 0;

            }
            
        }//completado
        private void listar_Variables()
        {
            int count = data_v.RowCount;
            for (int x = 0; x < count; x++)
            {
                if (Convert.ToString(data_v.Rows[x].Cells[1].Value) == "int")
                {
                    L_enteros.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                    L_general.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                }
                if (Convert.ToString(data_v.Rows[x].Cells[1].Value) == "string")
                {
                    L_string.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                    L_general.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                }
                if (Convert.ToString(data_v.Rows[x].Cells[1].Value) == "float")
                {
                    L_float.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                    L_general.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                }
                if (Convert.ToString(data_v.Rows[x].Cells[1].Value) == "bool")
                {
                    L_bool.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                    L_general.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                }
                if (Convert.ToString(data_v.Rows[x].Cells[1].Value) == "char")
                {
                    L_char.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                    L_general.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                }
                if (Convert.ToString(data_v.Rows[x].Cells[1].Value) == "double")
                {
                    L_dobles.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                    L_general.Add(Convert.ToString(data_v.Rows[x + 1].Cells[1].Value));
                }

            }
        }//completado
        private void validacion_PuntoComa(List<string> values,int posicion)
        {
            //El valor values obtiene el valor de un renglon

            int cont = values.Count();
            //MessageBox.Show(cont.ToString());
            if ( values[0]== "entero" || values[0]== "Variables" || values[0]== "numero flotante" ||
                values[0]== "valor booleano"|| values[0]== "cadena de caracteres" || values[0]== "letra"
                ||values[0]=="numero extendido")
            {
                if(values[cont-1]!="Punto y coma")
                {
                    txb_errores.Text = txb_errores.Text + "\n Se esperaba ; en la  line " + posicion;
                }
            }
        }//completado
        private void revisar_uso()
        {
            int largo= L_general.Count();
            int cont=0;

            for(int y = 0; y < largo; y++)
            {
                foreach (DataGridViewRow Row in data_v.Rows)
                {
                    if (L_general[y] == Convert.ToString(Row.Cells["Variable"].Value))
                    {
                        cont++;
                    }
                }
                if (cont == 1)
                {
                    txb_errores.Text = txb_errores.Text + "\n Warning la variable " + L_general[y]+" se encuentra declarada pero nunca se usa";
                }
                cont = 0;
            }
           
        }//completado 
        private void CompatibilidadDeTipos()
        {
            int number;
            int numero_fila = 1;
            List<string> values = new List<string>();
            List<string> var = new List<string>();
            List<string> final = new List<string>();

            foreach (DataGridViewRow Row in data_v.Rows)
            {

                string estado = Convert.ToString(Row.Cells["Estado"].Value);
                string variable = Convert.ToString(Row.Cells["Variable"].Value);
                string fila = Convert.ToString(Row.Cells["Fila"].Value);

                bool isParsable = Int32.TryParse(fila, out number);
                if (numero_fila == number)
                {
                    values.Add(estado);
                    var.Add(variable);
                }
                else
                {
                    //aqui revisar
                    int largo = values.Count();
                    for(int x = 0; x < largo; x++)
                    {
                        //MessageBox.Show(values[x].ToString());
                        if(values[x] == "Variables")
                        {
                        //    MessageBox.Show(var[x].ToString());
                      //      MessageBox.Show("entra");
                            final.Add(var[x]);
                            
                        }
                        
                    }
                    //MessageBox.Show(final.Count().ToString());
                    //MessageBox.Show("termina");
                    
                    if (final.Count() > 1)
                    {

                        int d_entero = 0;
                        int d_flotante = 0;
                        int d_string = 0;
                        int d_char = 0;
                        int d_double = 0;
                        int d_bool = 0;
                       
                        //aqui la busqueda uff xd
                        for(int x=0; x < final.Count(); x++)
                        {
                           
                            if(L_enteros.Exists(e => e.EndsWith (final[x])))
                            {
                                d_entero++;
                            }
                            if (L_float.Exists(e => e.EndsWith(final[x])))
                            {
                                d_flotante++;
                            }
                            if (L_dobles.Exists(e => e.EndsWith(final[x])))
                            {
                                d_double++;
                            }
                            if (L_string.Exists(e => e.EndsWith(final[x])))
                            {
                                d_string++;
                            }
                            if (L_char.Exists(e => e.EndsWith(final[x])))
                            {
                                d_char++;
                            }
                            if (L_bool.Exists(e => e.EndsWith(final[x])))
                            {
                                d_bool++;
                            }
                        }
                       

                        //aqui para ver los resultados 
                        if (d_char!=final.Count() && d_bool != final.Count() && d_string != final.Count() && d_flotante != final.Count() && d_entero != final.Count())
                        {
                            txb_errores.Text = txb_errores.Text + "\n Incopatibilidad de tipos en la linea " + numero_fila;
                        }
                           
                    }
                    numero_fila = numero_fila + 1;
                    values.Clear();
                    final.Clear();
                    var.Clear();
                    values.Add(estado);
                    var.Add(variable);
                }
            }
        }//completado
        private void validar()
        {
            int con_parentesis = 0;
            int con_parentesis_2 = 0;
            int con_llaves = 0;
            int con_llaves2 = 0;
            int number;
            int numero_fila = 1;
            //llamar al final de la funcion para comprobar que todo este bien 
            int cont=data_v.RowCount;// Para sacar el numero de filas en numero
            List<string> values = new List<string>();
            List<string> var = new List<string>();
            foreach (DataGridViewRow Row in data_v.Rows)
            {   
                //estan todod los valores asignados
                string estado = Convert.ToString(Row.Cells["Estado"].Value);
                string variable = Convert.ToString(Row.Cells["Variable"].Value);
                string columna = Convert.ToString(Row.Cells["Columna"].Value);
                string fila = Convert.ToString(Row.Cells["Fila"].Value);
                //corroborar el numero de parentesis y lllaves 
                if (variable == "(")
                {
                    con_parentesis = con_parentesis + 1;
                }
                if (variable == ")")
                {
                    con_parentesis_2 = con_parentesis_2 + 1;
                }
                if (variable == "{")
                {
                    con_llaves++;
                }
                if (variable == "}")
                {
                    con_llaves2++;
                }
                
                bool isParsable = Int32.TryParse(fila, out number);
                if (numero_fila == number)
                {
                    
                    values.Add(estado);
                  
                }
                else
                {
                    validacion_PuntoComa(values,numero_fila);//aqui va la funcion auxiliar
                    
                    formato_Correcto_variables(values,numero_fila);//para ver la entrada de variables
                    numero_fila = numero_fila + 1;
                    values.Clear();
                    values.Add(estado);
                }
                if (estado == "Variables")
                {
                    var.Add(variable);
                }
     
            }
            if (con_parentesis != con_parentesis_2)//evaluamos la cantidad de parentesis abiertos
            {
                txb_errores.Text = txb_errores.Text + "\n El numero de parentesis " +
                    "abiertos no coincide con el numero de parentesis cerrados";
            }
            if (con_llaves != con_llaves2)//evaluamos la cantidad de parentesis cerrados
            {
                txb_errores.Text = txb_errores.Text + "\n El numero de llaves " +
                    "abiertos no coincide con el numero de llaves cerrados";
            }
            
            validar_variables(var);
            correcion_float();//corregir el estado que desconoce 
            listar_Variables();
            validar_variables_repetidas();
            revisar_uso();
            //identificar_variables();
            variables_desconocidas();
            CompatibilidadDeTipos();
        }
        private string analizar(string analizado)
        {
            //MessageBox.Show(analizado);
            string dato = "";
            if (tipo== true)
            {
               
                dato = "Variables";
                tipo = false;
            }
            else
            {


                switch (analizado)
                {
                    case "=":
                        dato = "Signo igual";
                        break;
                    case "==":
                        dato = "comparacion";
                        break;
                    case "+":
                        dato = "Signo de suma";
                        break;
                    case "-":
                        dato = "Signo de resta";
                        break;
                    case "*":
                        dato = "Signo de multiplicacion";
                        break;
                    case "/":
                        dato = "division";
                        break;
                    case "main":
                        dato = "inicio";
                        break;
                    case "(":
                        dato = "parentesis izquierdo";
                        break;
                    case ")":
                        dato = "parentesis derecho";
                        break;
                    case "{":
                        dato = "llave apertura";
                        break;
                    case "}":
                        dato = "llave para cerrar";
                        break;
                    case "int":
                        dato = "entero";
                        tipo = true;
                        break;
                    case "INT":
                        tipo = true;
                        dato = "entero";
                        break;
                    case "float":
                        tipo = true;
                        dato = "numero flotante";
                        break;
                    case "bool":
                        tipo = true;
                        dato = "valor booleano";
                        break;
                    case "string":
                        tipo = true;
                        dato = "cadena de caracteres";
                        break;
                    case "char":
                        tipo = true;
                        dato = "letra";
                        break;
                    case "double":
                        tipo = true;
                        dato = "numero extendido";
                        break;
                    case "if":
                        dato = "sentencia if";
                        break;
                    case "for":
                        dato = "ciclo for";
                        break;
                    case "while":
                        dato = "coclio while";
                        break;
                    case "void":
                        dato = "numero extendido";
                        break;
                    case "return":
                        dato = "valor de retorno";
                        break;
                    case "#":
                        dato = "comentario";
                        break;
                    case ";":
                        dato = "Punto y coma";
                        break;
                    case ":":
                        dato = "dos puntos";
                        break;
                    case ".":
                        dato = "un punto";
                        break;
                    case "suma":
                        dato = "Funcion de ensamblador";
                        break;
                    case "resta":
                        dato = "Funcion de ensamblador";
                        break;
                    case "division":
                        dato = "Funcion de ensamblador";
                        break;
                    case "raiz":
                        dato = "Funcion de ensamblador";
                        break;
                    case "sin":
                        dato = "Funcion de ensamblador";
                        break;
                    case "cos":
                        dato = "Funcion de ensamblador";
                        break;
                    case "potencia":
                        dato = "Funcion de ensamblador";
                        break;
                    case "modulo":
                        dato = "Funcion de ensamblador";
                        break;
                    case "tangente":
                        dato = "Funcion de ensamblador";
                        break;
                    case "cout":
                        dato = "impresion";
                        break;
                    case "<<":
                        dato = "Operador";
                        break;
                    case "endl":
                        dato = "salto de linea";
                        break;
                    case "!=":
                        dato = "Diferencia";
                        break;
                    case "cin":
                        dato = "lectura de datos";
                        break;



                    default:
                        int valor = 0;
                        int validar = 0;
                        if (int.TryParse(analizado, out valor))
                        {
                            dato = "Numero";
                            validar = 1;
                        }
                        if (analizado.StartsWith("'") && analizado.EndsWith("'"))
                        {
                            dato = "texto";
                            validar = 1;
                        }
                        if (validar == 0)
                        {
                            dato = "desconocido";
                        }
                        break;
                }
            }
            return dato;
     }

        
        private void primera_funcion()
        {
            
            int posicion = 0;
            string REntrada = txb_codigo.Text;

            char[] salto = { '\n' };
            char[] limitador = { ' ' };
            string[] RArray = REntrada.Split(salto);
            for (int i = 0; i < RArray.Length; i++)
            {
                //para dividir el texto en lineas
                string[] palabra = RArray[i].Split(limitador);
                for (int j = 0; j < palabra.Length;  j++)
                {
                    palabra[j] = palabra[j].Replace("\n", "");
                    if (palabra[j] != "")
                    {
                        data_v.Rows.Add();
                        data_v.Rows[posicion].Cells[0].Value = analizar(palabra[j]);
                        data_v.Rows[posicion].Cells[1].Value = palabra[j];
                        data_v.Rows[posicion].Cells[2].Value = j + 1;//columna
                        data_v.Rows[posicion].Cells[3].Value = i + 1;//fila

                        posicion++;
                    }
                }

            }
            //validar();
            //MessageBox.Show(data_v.RowCount.ToString());
        }

        private void btn_limpiar_Click(object sender, EventArgs e)
        {
            this.data_v.DataSource = null;
            this.data_v.Rows.Clear();
            txb_codigo.Clear();
            txb_errores.Clear();
            L_general.Clear();
            L_char.Clear();
            L_dobles.Clear();
            L_enteros.Clear();
            L_float.Clear();
            L_string.Clear();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
          
       
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void data_v_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            string path = "C:\\Users\\chuyi\\OneDrive\\Documentos\\Rodriguez Reteria Jesus Alejandro\\Universidad\\sexto semestre\\Seminario de traducores 2\\ejemplo.cpp";
            
            //StreamWriter outpuFile = new StreamWriter("ejemplo.txt");
            //outpuFile.Write(txb_codigo.Text);
            
            File.WriteAllText(path, txb_codigo.Text);
            MessageBox.Show("El codigo se guardo con el nombre de ejemplo.cpp en la carpeta del proyecto");
        }

        private void txb_codigo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
