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
using System.Drawing;
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
		RomGba romGba;
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
		void btnLoadRom_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog opnFile=new OpenFileDialog();
			int offsetAsm;
			opnFile.Filter="Pokemon GBA|*.gba";
			if(opnFile.ShowDialog().GetValueOrDefault())
			{
				romGba=new RomGba(opnFile.FileName);
				try{
					switch (EdicionPokemon.GetEdicionPokemon(romGba).AbreviacionRom) {
						case AbreviacionCanon.AXV:
							imgEdicion.SetImage(ASM_Compiler.Resources.PokeballRuby);
							break;
						case AbreviacionCanon.AXP:
							imgEdicion.SetImage(ASM_Compiler.Resources.PokeballZafiro);
							break;
						case AbreviacionCanon.BPE:
							imgEdicion.SetImage(ASM_Compiler.Resources.PokeballEsmeralda);
							break;
						case AbreviacionCanon.BPR:
							imgEdicion.SetImage(ASM_Compiler.Resources.PokeballRojoFuego);
							break;
						case AbreviacionCanon.BPG:
							imgEdicion.SetImage(ASM_Compiler.Resources.PokeballVerdeHoja);
							break;
							
					}}
				catch{
					imgEdicion.SetImage(ASM_Compiler.Resources.GbaIcono);
				}
				txtNameRom.Text=romGba.Nombre;
				//pongo la imagen
				offsetAsm=   romGba.Data.SearchArray(asmToView.AsmBinary);
				if(offsetAsm>0)
				{
					btnPonerOQuitarBIN.Content="Quitar";
					txtAsmBinaryInRom.Text=((Gabriel.Cat.Hex)offsetAsm).ByteString;
				}else{
					btnPonerOQuitarBIN.Content="Insertar";
					txtAsmBinaryInRom.Text="";
				}}
			
			
		}
		void btnPonerOQuitarBIN_Click(object sender, RoutedEventArgs e)
		{
			if(btnPonerOQuitarBIN.Content.ToString()[0]=='Q')
			{
				//quito el binario de la rom
				romGba.Data.Remove(romGba.Data.SearchArray(asmToView.AsmBinary),asmToView.AsmBinary.Length,0x0);
				btnPonerOQuitarBIN.Content="Insertar";
				txtAsmBinaryInRom.Text="";
			}else{
				btnPonerOQuitarBIN.Content="Quitar";
				txtAsmBinaryInRom.Text=((Gabriel.Cat.Hex)romGba.Data.SetArray(asmToView.AsmBinary)).ByteString;
			}
			try{
				romGba.Save();
			}catch{
				MessageBox.Show("No se puede guardar en el archivo actual \n"+romGba.BackUp());
				
			}
		}

	}
}