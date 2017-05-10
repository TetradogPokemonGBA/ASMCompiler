/*
 * Creado por SharpDevelop.
 * Usuario: tetra
 * Fecha: 07/05/2017
 * Hora: 21:02
 * Licencia GNU GPL V3
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using Microsoft.Win32;
using PokemonGBAFrameWork;
using Gabriel.Cat.Extension;

namespace ASM_Compiler
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class Window1 : Window
	{
		ASM resultadoCompilado;
		public Window1()
		{
			MenuItem item=new MenuItem();
			item.Header="Save ASM code";
			item.Click+=(s,e)=>{		
				SaveASMCode(txtAsmToCompile.Text);
			};
			
			InitializeComponent();
			txtAsmToCompile.ContextMenu=new ContextMenu();
			txtAsmToCompile.ContextMenu.Items.Add(item);
			btnGuardar.IsEnabled = false;
			btnShowCompiledResult.IsEnabled = false;
			
			//para poder cargar un archivo arrastrado a dentro
			
			Drop+=DragEvent;
			AllowDrop=true;
			PreviewDragOver+=(s,e)=>e.Handled=true;
			
		}

		void DragEvent(object sender, DragEventArgs e)
		{
			bool hacerlo;
			if(String.IsNullOrEmpty(txtAsmToCompile.Text))
				hacerlo=true;
			else
			{
				hacerlo=MessageBox.Show("Quieres sustituir el texto que hay por el nuevo del archivo?","Atención, no se guardará lo que hay",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes;
				
			}
			if(hacerlo)
				txtAsmToCompile.Text=File.ReadAllText(((string[])e.Data.GetData(DataFormats.FileDrop))[0],System.Text.Encoding.Unicode);
		}

		void btnCompilar_Click(object sender, RoutedEventArgs e)
		{
			resultadoCompilado = ASM.Compilar(txtAsmToCompile.Text);
			if (resultadoCompilado.AsmBinary != null) {
				tbMensaje.Text = "Todo compilado correctamente";
				tbMensaje.Foreground = Brushes.GreenYellow;
				btnGuardar.IsEnabled = true;
				btnShowCompiledResult.IsEnabled = true;
			} else {
				if (!String.IsNullOrEmpty(resultadoCompilado.ErrorCompilar))
					tbMensaje.Text = resultadoCompilado.ErrorCompilar;
				else
					tbMensaje.Text = "¡Hay errores en el código!";
				tbMensaje.Foreground = Brushes.Red;
				btnGuardar.IsEnabled = false;
				btnShowCompiledResult.IsEnabled = false;
			}
			
		}
		void btnGuardar_Click(object sender, RoutedEventArgs e)
		{
			SaveASMBin(resultadoCompilado.AsmBinary);
		}
		public static void SaveASMBin(byte[] asmBin)
		{
			FileStream fs;
			BinaryWriter br;
			SaveFileDialog sfdCodigo = new SaveFileDialog();
			sfdCodigo.DefaultExt=".bin";
			if (sfdCodigo.ShowDialog().GetValueOrDefault()) {
				if(File.Exists(sfdCodigo.FileName))
					File.Delete(sfdCodigo.FileName);
				fs = new FileStream(sfdCodigo.FileName, FileMode.Create);
				br = new BinaryWriter(fs);
				br.BaseStream.Write(asmBin);
				br.Close();
				fs.Close();
			}
		}

		public static void SaveASMCode(string asmCode)
		{
			FileStream fs;
			BinaryWriter br;
			SaveFileDialog sfdCodigo = new SaveFileDialog();
			sfdCodigo.DefaultExt=".asm";
			if (sfdCodigo.ShowDialog().GetValueOrDefault()) {
				if(File.Exists(sfdCodigo.FileName))
					File.Delete(sfdCodigo.FileName);
				fs = new FileStream(sfdCodigo.FileName, FileMode.Create);
				br = new BinaryWriter(fs,Encoding.Unicode);
				br.BaseStream.Write(asmCode);
				br.Close();
				fs.Close();
			}
		}

		void btnShowCompiledResult_Click(object sender, RoutedEventArgs e)
		{
			new ASMView(resultadoCompilado).Show();
		}
	}
}