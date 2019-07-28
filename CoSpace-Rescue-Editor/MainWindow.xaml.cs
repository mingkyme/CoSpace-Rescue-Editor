using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Xml;
namespace CoSpace_Rescue_Editor
{
    public class XmlTreeViewItem : TreeViewItem
    {
        public XmlNode xml;
        public string NodeName
        {
            get
            {
                return xml["Name"].InnerText;
            }
            set
            {
                xml["Name"].InnerText = value;
            }
        }
        public string AdvancedCondition
        {
            get
            {
                return xml["State"]["Adv_State"].InnerText;
            }
            set
            {
                xml["State"]["Adv_State"].InnerText = value;
            }
        }
        public string AdvancedAction
        {
            get
            {
                return xml["Action"]["Adv_Action"].InnerText;
            }
            set
            {
                xml["Action"]["Adv_Action"].InnerText = value;
            }
        }
        public XmlTreeViewItem(XmlNode xmlNode)
        {
            xml = xmlNode;
            Header = NodeName;
        }
    }
    public partial class MainWindow : Window
    {
        
        
        public MainWindow()
        {
            InitializeComponent();
        }
        XmlDocument xml;
        string fileName;
        private void Open_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SMP file (*.smp)|*.smp";
            if (openFileDialog.ShowDialog() == true)
            {
                XAML_World1_TreeView.Items.Clear();
                XAML_World2_TreeView.Items.Clear();
                xml = new XmlDocument();
                fileName = openFileDialog.FileName;
                xml.Load(fileName);

                XmlNode world1XML = xml.SelectSingleNode("/SM_AI/SM_x0020_Nodes0"); //접근할 노드
                XmlNode world2XML = xml.SelectSingleNode("/SM_AI/SM_x0020_Nodes1"); //접근할 노드
                foreach (XmlNode i in world1XML.ChildNodes)
                {
                    XAML_World1_TreeView.Items.Add(new XmlTreeViewItem(i));
                }
                foreach (XmlNode i in world2XML.ChildNodes)
                {
                    XAML_World2_TreeView.Items.Add(new XmlTreeViewItem(i));
                }

            }

        }
        private void Save_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save_XmlTreeViewItem();
                xml.Save(fileName);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("파일을 먼저 불러와주세요");
            }
        }

        private void World_Button_Checked(object sender, RoutedEventArgs e)
        {
            if(XAML_World1_TreeView is null || XAML_World2_TreeView is null) { return; }
            Save_XmlTreeViewItem();
            XAML_World1_TreeView.Visibility = Visibility.Hidden;
            XAML_World2_TreeView.Visibility = Visibility.Hidden;

            try
            {
                (XAML_World1_TreeView.SelectedItem as TreeViewItem).IsSelected = false;
                (XAML_World2_TreeView.SelectedItem as TreeViewItem).IsSelected = false;
            }
            catch
            {

            }
            
            XAML_Advanced_Action_TextBox.Text = "";
            XAML_Advanced_Condition_TextBox.Text = "";
            preSelet = null;
            switch ((sender as Custom.CustomRadioButton).Text)
            {
                case "World1":
                    XAML_World1_TreeView.Visibility = Visibility.Visible;

                    break;
                case "World2":
                    XAML_World2_TreeView.Visibility = Visibility.Visible;

                    break;
            }
        }
        XmlTreeViewItem preSelet;
        private void XAML_World_TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Save_XmlTreeViewItem();
            if ( (sender as TreeView).SelectedItem is null )
            {
                return;
            }
            XAML_Advanced_Condition_TextBox.Text = ((sender as TreeView).SelectedItem as XmlTreeViewItem).AdvancedCondition;
            XAML_Advanced_Action_TextBox.Text = ((sender as TreeView).SelectedItem as XmlTreeViewItem).AdvancedAction;
            preSelet = (sender as TreeView).SelectedItem as XmlTreeViewItem;
        }
        private void Save_XmlTreeViewItem()
        {
            if(preSelet is null)
            {
                return;
            }
            preSelet.AdvancedCondition = XAML_Advanced_Condition_TextBox.Text;
            preSelet.AdvancedAction = XAML_Advanced_Action_TextBox.Text;
        }
    }
}
