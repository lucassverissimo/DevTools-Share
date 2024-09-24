public class MultiSelectComboBox : ComboBox
{
    private CheckedListBox checkedListBox;
    private TextBox searchBox;
    private Button dropDownButton;
    private bool dropdownOpened = false;

    public MultiSelectComboBox()
    {
        this.DropDownHeight = 1;  // Define um tamanho pequeno para o ComboBox padrão

        // Inicializa a lista de seleção múltipla
        checkedListBox = new CheckedListBox
        {
            CheckOnClick = true,
            BorderStyle = BorderStyle.FixedSingle,
            Visible = false
        };

        // Inicializa a caixa de pesquisa
        searchBox = new TextBox();
        searchBox.TextChanged += SearchBox_TextChanged;

        // Inicializa o botão de dropdown
        dropDownButton = new Button();
        dropDownButton.Text = "▼";
        dropDownButton.Width = 20;
        dropDownButton.Height = this.Height;
        dropDownButton.Top = 0;
        dropDownButton.Left = this.Width - dropDownButton.Width;
        dropDownButton.Click += (s, e) => { ToggleDropdown(); };

        // Adiciona o botão ao ComboBox
        this.Controls.Add(dropDownButton);

        // Evento para fechar o dropdown
        this.DropDownClosed += (s, e) => { CloseDropdown(); };
        this.Click += (s, e) => { ToggleDropdown(); };

        // Adiciona o evento de ItemCheck para atualização de texto
        checkedListBox.ItemCheck += checkedListBox_ItemCheck;
    }

    private void ToggleDropdown()
    {
        if (dropdownOpened)
        {
            CloseDropdown();
        }
        else
        {
            OpenDropdown();
        }
    }

    private void OpenDropdown()
    {
        if (!this.Parent.Controls.Contains(checkedListBox))
        {
            // Adiciona e posiciona os controles no formulário pai
            this.Parent.Controls.Add(checkedListBox);
            this.Parent.Controls.Add(searchBox);
            PositionControls();
        }

        checkedListBox.BringToFront();
        searchBox.BringToFront();
        checkedListBox.Visible = true;
        searchBox.Visible = true;
        dropdownOpened = true;
    }

    private void CloseDropdown()
    {
        checkedListBox.Visible = false;
        searchBox.Visible = false;
        dropdownOpened = false;

        // Atualiza o texto ao fechar o dropdown
        UpdateText();
    }

    // Reposiciona os controles dinamicamente
    private void PositionControls()
    {
        searchBox.SetBounds(this.Left, this.Bottom, this.Width, searchBox.PreferredHeight);
        checkedListBox.SetBounds(this.Left, searchBox.Bottom, this.Width, 200);  // Ajusta altura conforme necessário

        // Garante que o checkedListBox fique no topo
        checkedListBox.BringToFront();
        searchBox.BringToFront();
    }

    private void SearchBox_TextChanged(object sender, EventArgs e)
    {
        string searchText = searchBox.Text.ToLower();
        for (int i = 0; i < checkedListBox.Items.Count; i++)
        {
            string itemText = checkedListBox.Items[i].ToString().ToLower();
            bool matches = itemText.Contains(searchText);
            checkedListBox.SetItemChecked(i, matches && checkedListBox.GetItemChecked(i));  // Mantém itens selecionados
        }
    }

    // Preenche o ComboBox com uma lista genérica de itens
    public void SetItems<T>(List<T> items)
    {
        checkedListBox.Items.Clear();
        foreach (var item in items)
        {
            checkedListBox.Items.Add(item);
        }
    }

    // Obtém os itens selecionados
    public List<T> GetSelectedItems<T>()
    {
        List<T> selectedItems = new List<T>();
        foreach (var item in checkedListBox.CheckedItems)
        {
            selectedItems.Add((T)item);
        }
        return selectedItems;
    }

    // Atualiza o texto do ComboBox para mostrar os itens selecionados
    private void UpdateText()
    {
        List<string> selectedItems = checkedListBox.CheckedItems.Cast<object>()
            .Select(i => i.ToString()).ToList();

        if (selectedItems.Count > 0)
        {
            // Mostra quantos itens foram selecionados
            this.Text = $"{selectedItems.Count} itens selecionados";
        }
        else
        {
            this.Text = "Selecione itens";
        }
    }

    // Evento para atualização de texto ao selecionar/deselecionar itens
    private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
    {
        // Usa BeginInvoke para garantir que o ItemCheck seja processado corretamente antes de atualizar o texto
        this.BeginInvoke(new Action(UpdateText));
    }
}
