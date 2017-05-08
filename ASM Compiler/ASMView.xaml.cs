/*
 * Creado por SharpDevelop.
 * Usuario: tetra
 * Fecha: 05/08/2017
 * Hora: 02:13
 * Licencia GNU GPL V3
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using PokemonGBAFrameWork;
using Gabriel.Cat.Extension;

namespace ASM_Compiler
{
	/// <summary>
	/// Interaction logic for ASMView.xaml
	/// </summary>
	public partial class ASMView : Window
	{
		ASM asmToView;
		public ASMView(ASM asmToView)
		{
			ContextMenu menu=new ContextMenu();
			MenuItem item=new MenuItem();
			item.Header="Save Binary output";
			item.Click+=(s,e)=>Window1.SaveASM(asmToView);
			menu.Items.Add(item);
			ContextMenu=menu;
			this.asmToView=asmToView;
			InitializeComponent();
			txtAsmCode.Text=asmToView.AsmCode;
			txtAsmBinary.Text=(Gabriel.Cat.Hex)asmToView.AsmBinary;
		}

		public ASM AsmToView {
			get {
				return asmToView;
			}
		}

	}
}