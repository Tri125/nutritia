﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Nutritia.UI.Views;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;

namespace Nutritia
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IApplicationService
	{
		private const int SLEEP_NOTIFICATION_TIME = 500;

		// Thread asincrone qui regarde les modifications dans la BD
		private Thread tNotif { get; set; }
		private Thread ThreadNotif { get; set; }

		// Permet d'interargir avec le threas asyncrone pour sauvegardé les nouveau plats dan la BD
		static public List<Plat> NouveauxPlats { get; set; }
		public string MessageNouvelleVersion { get; set; }

		private IPlatService platAsync;
		public List<Plat> LstPlat { get; set; }


		public MainWindow()
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(Properties.Settings.Default.Langue);
			InitializeComponent();
			ConfigurerTaille();
			Configurer();
			platAsync = new MySqlPlatService();
			MessageNouvelleVersion = "";
			NouveauxPlats = new List<Plat>();
			LstPlat = ServiceFactory.Instance.GetService<IPlatService>().RetrieveAll().ToList();

			presenteurContenu.Content = new MenuPrincipal();

			// On lance le thread
			tNotif = new Thread(VerifierChangementBD);
			tNotif.Start();
		}


		/// <summary>
		/// Méthode appellé en asyncrone qui boucle toute les demi-secondes pour verifier s'il y a des changements dans la BD
		/// </summary>
		private void VerifierChangementBD()
		{
			LstPlat = platAsync.RetrieveAll().ToList();
			List<int> lstIdNouveauPlat = new List<int>();
			List<Plat> ancienneLstPlat = platAsync.RetrieveAll().ToList();
			// Roule tant que l'utilisateur est connécté 
			while (true)
			{
				if (!String.IsNullOrEmpty(App.MembreCourant.NomUtilisateur))
				{
					bool ancienStatutAdmin = App.MembreCourant.EstAdministrateur;
					bool ancienStatutBanni = App.MembreCourant.EstBanni;

					App.MembreCourant = ServiceFactory.Instance.GetService<IMembreService>().Retrieve(new RetrieveMembreArgs { IdMembre = App.MembreCourant.IdMembre });

					LstPlat = platAsync.RetrieveAll().ToList();

					if (ancienneLstPlat.Count != LstPlat.Count)
					{
						Dispatcher.Invoke(AppliquerNouveauChangementStatut);
						ancienneLstPlat = platAsync.RetrieveAll().ToList();
					}

					foreach (var plat in LstPlat)
					{
						if (plat.NbVotes != ancienneLstPlat.Find(p => p.IdPlat == plat.IdPlat).NbVotes)
						{
							Dispatcher.Invoke(AppliquerNouveauChangementStatut);
							ancienneLstPlat = platAsync.RetrieveAll().ToList();
						}
					}

					if (App.MembreCourant.EstAdministrateur != ancienStatutAdmin
						|| App.MembreCourant.EstBanni != ancienStatutBanni)
					{
						Dispatcher.Invoke(AppliquerNouveauChangementStatut);
					}

					if (App.MembreCourant.DerniereMaj != "")
					{
						lstIdNouveauPlat.Clear();

						string str = "";
						foreach (var plat in LstPlat)
						{
							if (plat.DateAjout.CompareTo(App.MembreCourant.DerniereMaj) > -1)
							{
								if (!lstIdNouveauPlat.Exists(id => id == plat.IdPlat))
									lstIdNouveauPlat.Add((int)plat.IdPlat);
							}
							str += plat.Nom + " (" + plat.DateAjout + ")\n";
							
						}
						//MessageBox.Show(str);
						if (lstIdNouveauPlat.Count > 0)
						{
							List<Plat> lstPlatTemp = LstPlat.FindAll(plat => lstIdNouveauPlat.Contains((int)plat.IdPlat));

							if (NouveauxPlats.Count != lstPlatTemp.Count)
							{
								NouveauxPlats = lstPlatTemp;
								Dispatcher.Invoke(DessinerNotification);
							}
						}
					}
				}
				else
				{
					NouveauxPlats.Clear();
					Dispatcher.Invoke(DessinerNotification);
				}

				Thread.Sleep(SLEEP_NOTIFICATION_TIME); // On fait une pause pour ne pas surcharger le CPU

			}
		}


		private void ConfigurerTaille()
		{
			Width = App.APP_WIDTH;
			Height = App.APP_HEIGHT;
			MinWidth = App.APP_WIDTH;
			MinHeight = App.APP_HEIGHT;
		}

		/// <summary>
		/// Charge toutes les ressources du service factory
		/// </summary>
		public void Configurer()
		{
			// Inscription des différents services de l'application dans le ServiceFactory.
			ServiceFactory.Instance.Register<IRestrictionAlimentaireService, MySqlRestrictionAlimentaireService>(new MySqlRestrictionAlimentaireService());
			ServiceFactory.Instance.Register<IObjectifService, MySqlObjectifService>(new MySqlObjectifService());
			ServiceFactory.Instance.Register<IUniteMesureService, MySqlUniteMesureService>(new MySqlUniteMesureService());
			ServiceFactory.Instance.Register<IPreferenceService, MySqlPreferenceService>(new MySqlPreferenceService());
			ServiceFactory.Instance.Register<IAlimentService, MySqlAlimentService>(new MySqlAlimentService());
			ServiceFactory.Instance.Register<IPlatService, MySqlPlatService>(new MySqlPlatService());
			ServiceFactory.Instance.Register<IMenuService, MySqlMenuService>(new MySqlMenuService());
			ServiceFactory.Instance.Register<IMembreService, MySqlMembreService>(new MySqlMembreService());
			ServiceFactory.Instance.Register<IVersionLogicielService, MySqlVersionLogicielService>(new MySqlVersionLogicielService());
			ServiceFactory.Instance.Register<IApplicationService, MainWindow>(this);
		}

		/// <summary>
		/// Méthode qui permet de changer de userControl
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="vue"></param>
		public void ChangerVue<T>(T vue)
		{
			presenteurContenu.Content = vue as UserControl;
		}

		/// <summary>
		/// Ouvre la fenêtre des paramètres en modal
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnParam_Click(object sender, RoutedEventArgs e)
		{
			(new FenetreParametres()).ShowDialog();
		}

		/// <summary>
		/// Permet, suivant le contexte, de revenir au menu précédent
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRetour_Click(object sender, RoutedEventArgs e)
		{
			if (App.MembreCourant.IdMembre == null)
				ServiceFactory.Instance.GetService<IApplicationService>().ChangerVue(new MenuPrincipal());
			else
			{
				if (presenteurContenu.Content is Bannissement || presenteurContenu.Content is GestionAdmin || presenteurContenu.Content is GestionRepertoire)
					ServiceFactory.Instance.GetService<IApplicationService>().ChangerVue(new MenuAdministrateur());
				else
					ServiceFactory.Instance.GetService<IApplicationService>().ChangerVue(new MenuPrincipalConnecte());
			}
		}

		/// <summary>
		/// Ouvre la fenêtre des infos sur l'application en modal
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAPropos_Click(object sender, RoutedEventArgs e)
		{
			(new FenetreAPropos()).ShowDialog();
			//ServiceFactory.Instance.GetService<IApplicationService>().ChangerVue(new FenetreAPropos());
		}

		/// <summary>
		/// Ouvre la fenêtre d'aide de l'application en modeless
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAide_Click(object sender, RoutedEventArgs e)
		{
			FenetreAide fenetreAide = new FenetreAide(presenteurContenu.Content.GetType().Name);
			fenetreAide.Show();
		}

		/// <summary>
		/// Quand la fenêtre se charge, 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ThreadNotif = new Thread(LastestVersionPopUp);
			ThreadNotif.Start();
		}

		private void LastestVersionPopUp()
		{
			bool isOlder = false;
			IVersionLogicielService serviceMembre = ServiceFactory.Instance.GetService<IVersionLogicielService>();

			VersionLogiciel latestVersionLogiciel = serviceMembre.RetrieveLatest();

			if (String.IsNullOrEmpty(latestVersionLogiciel.Version))
			{
				return;
			}

			Version versionBD = new Version(latestVersionLogiciel.Version);
			Version versionLocal = new Version(FileVersionInfo.GetVersionInfo(App.ResourceAssembly.Location).FileVersion);

			if (versionBD.Major > versionLocal.Major || versionBD.Minor > versionLocal.Minor || versionBD.Build > versionLocal.Build || versionBD.Revision > versionLocal.Revision)
			{
				isOlder = true;
			}


			Thread.Sleep(SLEEP_NOTIFICATION_TIME);

			if (isOlder)
			{
				MessageNouvelleVersion = "Version: " + latestVersionLogiciel.Version + " disponible.\nLien de téléchargement: " + latestVersionLogiciel.DownloadLink;
				Dispatcher.Invoke(DessinerNotification);
			}
		}

		private void btnNotification_Click(object sender, RoutedEventArgs e)
		{
			/*if (!String.IsNullOrEmpty(App.MembreCourant.NomUtilisateur))
			{
				if (NouveauxPlats.Count > 0)
				{
					NouveauxPlats.Clear();
					nbrNotif.Text = "";
					App.MembreCourant.DerniereMaj = DateTime.Now.ToString();
					ServiceFactory.Instance.GetService<IMembreService>().Update(App.MembreCourant);
					CreerBoiteNotification();
				}
			}*/
		}

		/// <summary>
		/// Formate les nouveaux plats pour les affichers lors du clique sur le bouton de notif
		/// </summary>
		private void CreerBoiteNotification()
		{
			StringBuilder sbNotifs = new StringBuilder();
			sbNotifs.AppendLine("     Nouveaux plats").AppendLine("-----------------------------------------").AppendLine();
			foreach (var plat in NouveauxPlats)
			{
				sbNotifs.Append("+ ").AppendLine(plat.Nom).AppendLine();
			}

			sbNotifs.AppendLine("-----------------------------------------").AppendLine("Voulez-vous ouvrir ces ").AppendLine("plats avec la calculatrice ?");

			if(MessageBox.Show(
					 sbNotifs.ToString(),
					 "Nouveaux plats disponibles",
					 MessageBoxButton.YesNo,
					 MessageBoxImage.Information) == MessageBoxResult.Yes)
			{
				// Si l'utilisateur à répondu oui pour afficher les plats dans la calculatrice ...<
				Window windowCalculatrice = new Window();
				windowCalculatrice.Width = App.APP_WIDTH;
				windowCalculatrice.Height = App.APP_HEIGHT;
				windowCalculatrice.ResizeMode = ResizeMode.CanMinimize;
				windowCalculatrice.Title = "Nutritia - Calculatrice nutritionnelle";
				windowCalculatrice.Icon = new BitmapImage(new Uri("pack://application:,,,/UI/Images/logoIconPetit.png"));
				Grid grdContenu = new Grid();
				ImageBrush brush = new ImageBrush();
				brush.Opacity = 0.3;
				brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/UI/Images/background.jpg"));
				grdContenu.Background = brush;
				grdContenu.Children.Add(new FenetreCalculatriceNutritionelle(NouveauxPlats));
				windowCalculatrice.Content = grdContenu;
				windowCalculatrice.Show();
			} 
			

			
		}

		/// <summary>
		/// Génere le nbr de notification pour le dessiner à côté de l'image de notif
		/// </summary>
		private void DessinerNotification()
		{
			if (!String.IsNullOrEmpty(App.MembreCourant.NomUtilisateur))
			{
				if (NouveauxPlats.Count > 0)
				{
					Button btnNouveauxPlats = new Button();
					Label lblNvPlat = new Label();
					lblNvPlat.Content = "Nouveaux plats disponibles (";
					lblNvPlat.Content += NouveauxPlats.Count.ToString() + ")";
					btnNouveauxPlats.Content = lblNvPlat;
					btnNouveauxPlats.Click += btnNouveauxPlats_Click;
					toolBarNotif.Items.Add(btnNouveauxPlats);

					MettreAJourNbrNotif();
				}
			}

			if (MessageNouvelleVersion != "")
			{
				Button btnNouvelleVersion = new Button();
				Label lblNvPlat = new Label();
				lblNvPlat.Content = "Nouveaux plats disponibles (";
				lblNvPlat.Content += NouveauxPlats.Count.ToString() + ")";
				btnNouvelleVersion.Content = lblNvPlat;
				btnNouvelleVersion.Click += btnNouvelleVersion_Click;
				toolBarNotif.Items.Add(btnNouvelleVersion);

				MettreAJourNbrNotif();
			}

		}

		public void AppliquerNouveauChangementStatut()
		{
			if (!String.IsNullOrEmpty(App.MembreCourant.NomUtilisateur))
			{
				if (presenteurContenu.Content is FenetreVotes)
					((FenetreVotes)presenteurContenu.Content).Rafraichir();

				if (App.MembreCourant.EstBanni)
				{
					App.MembreCourant = new Membre();
					toolBarNotif.Items.Clear();
					DessinerNotification();
					ServiceFactory.Instance.GetService<IApplicationService>().ChangerVue(new MenuPrincipal());
				}
				else
				{
					if (presenteurContenu.Content is MenuPrincipalConnecte)
						ServiceFactory.Instance.GetService<IApplicationService>().ChangerVue(new MenuPrincipalConnecte());
				}

				if (App.MembreCourant.EstAdministrateur)
				{
					if (presenteurContenu.Content is MenuPrincipalConnecte)
						ServiceFactory.Instance.GetService<IApplicationService>().ChangerVue(new MenuPrincipalConnecte());
				}
				else
				{
					if (presenteurContenu.Content is Bannissement
					|| presenteurContenu.Content is GestionAdmin
					|| presenteurContenu.Content is GestionRepertoire
					|| presenteurContenu.Content is MenuAdministrateur)
						ServiceFactory.Instance.GetService<IApplicationService>().ChangerVue(new MenuPrincipalConnecte());
				}

			}

		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			tNotif.Abort();
			ThreadNotif.Abort();
		}

		private void btnNouvelleVersion_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show(
                     MessageNouvelleVersion,
                     "Nouvelle version disponible",
                     MessageBoxButton.OK,
                     MessageBoxImage.Information);

			toolBarNotif.Items.Remove((Button)sender);

			MettreAJourNbrNotif();
		}

		private void btnNouveauxPlats_Click(object sender, RoutedEventArgs e)
		{
			CreerBoiteNotification();
			toolBarNotif.Items.Remove((Button)sender);
			
			App.MembreCourant.DerniereMaj = DateTime.Now.ToString();
			ServiceFactory.Instance.GetService<IMembreService>().Update(App.MembreCourant);

			MettreAJourNbrNotif();
		}

		private void MettreAJourNbrNotif()
		{
			string txtNbrNotif = (toolBarNotif.Items.Count - 1).ToString(); // Moins l'icone de notification

			if (txtNbrNotif == "0")
				txtNbrNotif = "";
			nbrNotif.Text = txtNbrNotif;
		}

	}
}
