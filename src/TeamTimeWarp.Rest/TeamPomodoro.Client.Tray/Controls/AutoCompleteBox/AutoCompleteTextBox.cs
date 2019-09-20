using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Diagnostics;

namespace TimeManager.Client.Tray.Controls
{

    //public class MyDataTemplateSelector : DataTemplateSelector
    //{
    //    public override DataTemplate SelectTemplate(
    //        object item,
    //        DependencyObject container)
    //    {
    //        Window wnd = Application.Current.MainWindow;
    //        if (item is string)
    //            return wnd.FindResource("WaitTemplate") as DataTemplate;
    //        else
    //            return wnd.FindResource("TheItemTemplate") as DataTemplate;
    //    }
    //}

    public class AutoCompleteTextBox : TextBox
    {
        Popup _popup;
        ListBox _listBox;
        Func<object, string, bool> _filter;
        string _textCache = "";
        bool _suppressEvent;
        // Binding hack - not really necessary.
        //DependencyObject dummy = new DependencyObject();
        readonly FrameworkElement _dummy = new FrameworkElement();

        public Func<object, string, bool> Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (_filter == value) return;
                _filter = value;
                
                if (_listBox != null)
                {
                    if (_filter != null)
                        _listBox.Items.Filter = FilterFunc;
                    else
                        _listBox.Items.Filter = null;
                }
            }
        }

        

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            ItemsControl.ItemsSourceProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemsSourceChanged(e.NewValue as IEnumerable);
        }

        protected void OnItemsSourceChanged(IEnumerable itemsSource)
        {
            if (_listBox == null) return;
            Debug.Print("Data: " + itemsSource);
            if (itemsSource is ListCollectionView)
            {
                _listBox.ItemsSource = new LimitedListCollectionView(((ListCollectionView)itemsSource).SourceCollection) { Limit = MaxCompletions };
                Debug.Print("Was ListCollectionView");
            }
            else if (itemsSource is CollectionView)
            {
                _listBox.ItemsSource = new LimitedListCollectionView(((CollectionView)itemsSource).SourceCollection) { Limit = MaxCompletions };
                Debug.Print("Was CollectionView");
            }
            else if (itemsSource is IList)
            {
                _listBox.ItemsSource = new LimitedListCollectionView(itemsSource) { Limit = MaxCompletions };
                Debug.Print("Was IList");
            }
            else
            {
                if (itemsSource == null)
                    itemsSource = Enumerable.Empty<object>();

                _listBox.ItemsSource = new LimitedCollectionView(itemsSource) { Limit = MaxCompletions };
                Debug.Print("Was IEnumerable");
            }
            if (_listBox.Items.Count == 0) InternalClosePopup();
        }

        public string Binding
        {
            get { return (string)GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Binding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.Register("Binding", typeof(string), typeof(AutoCompleteTextBox), new UIPropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            ItemsControl.ItemTemplateProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemTemplateChanged));

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemTemplateChanged(e.NewValue as DataTemplate);
        }

        private void OnItemTemplateChanged(DataTemplate p)
        {
            if (_listBox == null) return;
            _listBox.ItemTemplate = p;
        }

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemContainerStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemContainerStyleProperty =
            ItemsControl.ItemContainerStyleProperty.AddOwner(
                typeof(AutoCompleteTextBox),
                new UIPropertyMetadata(null, OnItemContainerStyleChanged));

        private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemContainerStyleChanged(e.NewValue as Style);
        }

        private void OnItemContainerStyleChanged(Style p)
        {
            if (_listBox == null) return;
            _listBox.ItemContainerStyle = p;
        }


        public object Selected
        {
            get { return (int)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }


        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(object), typeof(AutoCompleteTextBox), new UIPropertyMetadata());


        public int MaxCompletions
        {
            get { return (int)GetValue(MaxCompletionsProperty); }
            set { SetValue(MaxCompletionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxCompletions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxCompletionsProperty =
            DependencyProperty.Register("MaxCompletions", typeof(int), typeof(AutoCompleteTextBox), new UIPropertyMetadata(int.MaxValue));

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            ItemsControl.ItemTemplateSelectorProperty.AddOwner(typeof(AutoCompleteTextBox), new UIPropertyMetadata(null, OnItemTemplateSelectorChanged));

        private static void OnItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoCompleteTextBox actb = d as AutoCompleteTextBox;
            if (actb == null) return;
            actb.OnItemTemplateSelectorChanged(e.NewValue as DataTemplateSelector);
        }

        private void OnItemTemplateSelectorChanged(DataTemplateSelector p)
        {
            if (_listBox == null) return;
            _listBox.ItemTemplateSelector = p;
        }

        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        private void InternalClosePopup()
        {
            if (_popup != null)
                _popup.IsOpen = false;
        }
        private void InternalOpenPopup()
        {
            _popup.IsOpen = true;
            if (_listBox != null) _listBox.SelectedIndex = -1;
        }
        public void ShowPopup()
        {
            if (_listBox == null || _popup == null) InternalClosePopup();
            else if (_listBox.Items.Count == 0) InternalClosePopup();
            else InternalOpenPopup();
        }
        private void SetTextValueBySelection(object obj, bool moveFocus)
        {
            if (_popup != null)
            {
                InternalClosePopup();
                Dispatcher.Invoke((Action)(delegate
                {
                    Focus();
                    if (moveFocus)
                        MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }), System.Windows.Threading.DispatcherPriority.Background);
            }

            // Retrieve the Binding object from the control.
            var originalBinding = BindingOperations.GetBinding(this, BindingProperty);
            if (originalBinding == null) return;

            // Binding hack - not really necessary.
            //Binding newBinding = new Binding()
            //{
            //    Path = new PropertyPath(originalBinding.Path.Path, originalBinding.Path.PathParameters),
            //    XPath = originalBinding.XPath,
            //    Converter = originalBinding.Converter,
            //    ConverterParameter = originalBinding.ConverterParameter,
            //    ConverterCulture = originalBinding.ConverterCulture,
            //    StringFormat = originalBinding.StringFormat,
            //    TargetNullValue = originalBinding.TargetNullValue,
            //    FallbackValue = originalBinding.FallbackValue
            //};
            //newBinding.Source = obj;
            //BindingOperations.SetBinding(dummy, TextProperty, newBinding);

            // Set the dummy's DataContext to our selected object.
            _dummy.DataContext = obj;

            // Apply the binding to the dummy FrameworkElement.
            BindingOperations.SetBinding(_dummy, TextProperty, originalBinding);
            _suppressEvent = true;

            // Get the binding's resulting value.
            Text = _dummy.GetValue(TextProperty).ToString();
            _suppressEvent = false;
            _listBox.SelectedIndex = -1;
            SelectAll();
            Selected = obj;
        }

       

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (_suppressEvent) return;
            _textCache = Text ?? "";
            Debug.Print("Text: " + _textCache);
            if (_popup != null && _textCache == "")
            {
                InternalClosePopup();
            }
            else if (_listBox != null)
            {
                if (_filter != null)
                    _listBox.Items.Filter = FilterFunc;

                if (_popup != null)
                {
                    if (_listBox.Items.Count == 0)
                        InternalClosePopup();
                    else
                        InternalOpenPopup();
                }
            }
        }

        private bool FilterFunc(object obj)
        {
            return _filter(obj, _textCache);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _popup = Template.FindName("PART_Popup", this) as Popup;
            _listBox = Template.FindName("PART_ListBox", this) as ListBox;
            if (_listBox != null)
            {
                _listBox.PreviewMouseDown += HandleListBoxMouseUp;
                _listBox.KeyDown += HandleListBoxKeyDown;
                OnItemsSourceChanged(ItemsSource);
                OnItemTemplateChanged(ItemTemplate);
                OnItemContainerStyleChanged(ItemContainerStyle);
                OnItemTemplateSelectorChanged(ItemTemplateSelector);
                if (_filter != null)
                    _listBox.Items.Filter = FilterFunc;
            }
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (_suppressEvent) return;
            if (_popup != null)
            {
                InternalClosePopup();
            }
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            var fs = FocusManager.GetFocusScope(this);
            var o = FocusManager.GetFocusedElement(fs);
            if (e.Key == Key.Escape)
            {
                InternalClosePopup();
                Focus();
            }
            else if (e.Key == Key.Down)
            {
                if (_listBox != null && o == this)
                {
                    _suppressEvent = true;
                    _listBox.Focus();
                    _suppressEvent = false;
                }
            }
        }

        void HandleListBoxMouseUp(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListBoxItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null) return;
            var item = _listBox.ItemContainerGenerator.ItemFromContainer(dep);
            if (item == null) return;
            SetTextValueBySelection(item, false);
        }

        void HandleListBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
                SetTextValueBySelection(_listBox.SelectedItem, false);
            else if (e.Key == Key.Tab)
                SetTextValueBySelection(_listBox.SelectedItem, true);
        }
    }
}
