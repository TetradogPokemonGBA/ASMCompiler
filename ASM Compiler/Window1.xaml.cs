﻿/*
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
			InitializeComponent();
			btnGuardar.IsEnabled = false;
			btnShowCompiledResult.IsEnabled=false;
		}
		void btnCompilar_Click(object sender, RoutedEventArgs e)
		{
			tbMensaje.Text = "Compilando";
			tbMensaje.Foreground = Brushes.Blue;

			resultadoCompilado = ASM.Compilar(txtAsmToCompile.Text);
			if (resultadoCompilado.AsmBinary != null) {
				tbMensaje.Text = "Todo compilado correctamente";
				tbMensaje.Foreground = Brushes.GreenYellow;
				btnGuardar.IsEnabled = true;
				btnShowCompiledResult.IsEnabled=true;
			} else {
				tbMensaje.Text = resultadoCompilado.ErrorCompilar;
				tbMensaje.Foreground = Brushes.Red;
				btnGuardar.IsEnabled = false;
				btnShowCompiledResult.IsEnabled=false;
			}
			
		}
		void btnGuardar_Click(object sender, RoutedEventArgs e)
		{
			FileStream fs;
			BinaryWriter br;
			SaveFileDialog sfdCodigo = new SaveFileDialog();
			if (sfdCodigo.ShowDialog().GetValueOrDefault()) {
				fs = new FileStream(sfdCodigo.FileName, FileMode.CreateNew);
				br = new BinaryWriter(fs);
				br.BaseStream.Write(resultadoCompilado.AsmBinary);
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