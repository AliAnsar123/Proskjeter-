package org.example;

import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.concurrent.WorkerStateEvent;
import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.scene.control.cell.TextFieldTableCell;
import javafx.scene.layout.AnchorPane;
import javafx.scene.layout.GridPane;
import javafx.scene.layout.VBox;
import javafx.stage.FileChooser;
import javafx.stage.Stage;
import javafx.util.Callback;
import org.example.exceptions.InvalidCategoryException;
import org.example.exceptions.InvalidCategoryOptionException;
import org.example.exceptions.InvalidNameException;

import java.io.File;
import java.nio.file.Path;
import java.util.stream.Collectors;

public class PrimaryController {
    public static ObservableList<Category> categoryRegister = FXCollections.observableArrayList();
    public static ObservableList<Product> productRegister = FXCollections.observableArrayList();
    public static ObservableList<Product> filteredProductRegister = FXCollections.observableArrayList();

    @FXML private TextField newCategoryTextField;
    @FXML private TextField newCategoryOptionTextField;
    @FXML private TableView<Category> categoryTableView;
    @FXML private TableView<CategoryOption> categoryOptionTableView;
    @FXML private TableView<Product> productTableView;
    @FXML private ComboBox<Category> categoryComboBox;
    @FXML private TextField newProductTextField;
    @FXML private ComboBox<Category> filterCategoryComboBox;
    @FXML private ComboBox<CategoryOption> filterOptionComboBox;
    @FXML private TextField filterTextField;
    @FXML private VBox vbox;
    @FXML private GridPane gridPane;
    @FXML private AnchorPane anchorPane;


    private Category selectedCategory;
    private Threads task;


    @FXML
    public void initialize() {
        categoryTableView.setItems(categoryRegister);
        categoryComboBox.setItems(categoryRegister);
        filterCategoryComboBox.setItems(categoryRegister);
        productTableView.setItems(filteredProductRegister);

        TableColumn deleteButtonColumn = new TableColumn<>("Slett");
        Callback<TableColumn<?, String>, TableCell<?, String>> cellFactory = new DeleteButtonCellFactory(categoryTableView, categoryRegister, this::filter).cellFactory;
        deleteButtonColumn.setCellFactory(cellFactory);
        deleteButtonColumn.setPrefWidth(36.0);
        categoryTableView.getColumns().add(deleteButtonColumn);
    }

    @FXML
    private void newCategory(Category category) {
        try {
            categoryRegister.add(category);
            newCategoryTextField.clear();
        } catch (InvalidNameException | InvalidCategoryException e) {
            System.out.println(e.getMessage());
        }
    }
    @FXML
    private void newComponentButton() {
        // lager en ny, blank komponent for utfylling
        Category category = new Category(newCategoryTextField.getText());
        newCategory(category);
        // filtrerer komponenter på nytt når ny komponent legges til
    }

    @FXML
    private void newCategoryOption() {
        try {
            CategoryOption CategoryOption = new CategoryOption(selectedCategory, newCategoryOptionTextField.getText());
            selectedCategory.addCategoryOption(CategoryOption);
            newCategoryOptionTextField.clear();
            filter();
        } catch (InvalidCategoryOptionException | InvalidNameException e) {
            System.out.println(e.getMessage());
        } catch (NullPointerException e) {
            System.out.println("Du må velge en kategori");
        }
    }

    @FXML
    private void categoryNameEdited(TableColumn.CellEditEvent<Category, ?> event) {
        try {
            event.getRowValue().setName(event.getNewValue().toString());
        } catch (InvalidCategoryException | InvalidNameException e) {
            System.out.println(e.getMessage());
        }
        event.getTableView().refresh();
        filter();
    }

    @FXML
    private void categoryOptionNameEdited(TableColumn.CellEditEvent<CategoryOption, ?> event) {
        try {
            event.getRowValue().setName(event.getNewValue().toString());
        } catch (InvalidCategoryOptionException | InvalidNameException e) {
            System.out.println(e.getMessage());
        }
        event.getTableView().refresh();
        filter();
    }

    @FXML
    private void onCategoryTableViewEdit() {
        if (categoryTableView.getSelectionModel().getSelectedItem() != null) {
            selectedCategory = categoryTableView.getSelectionModel().getSelectedItem();
            categoryOptionTableView.setItems(selectedCategory.getOptions());

            TableColumn deleteButtonColumn = new TableColumn<>("Slett");
            Callback<TableColumn<?, String>, TableCell<?, String>> cellFactory = new DeleteButtonCellFactory(categoryOptionTableView, selectedCategory.getOptions(), this::filter).cellFactory;
            deleteButtonColumn.setCellFactory(cellFactory);
            deleteButtonColumn.setPrefWidth(36.0);
            // replaces old column connected to the previously selected category
            if (categoryOptionTableView.getColumns().size() > 1)
                categoryOptionTableView.getColumns().remove(1);

            categoryOptionTableView.getColumns().add(deleteButtonColumn);
        }
    }

    @FXML
    private void newProduct() {
        try {
            Product product = new Product(newProductTextField.getText(), categoryComboBox.getValue());
            productRegister.add(product);
            filter();
            newProductTextField.clear();
        } catch (InvalidNameException | InvalidCategoryException e) {
            System.out.println(e.getMessage());
        } catch (NullPointerException e) {
            System.out.println("Du må velge en kategori");
        }
    }

    @FXML
    private void productNameEdited(TableColumn.CellEditEvent<Product, ?> event) {
        try {
            event.getRowValue().setName(event.getNewValue().toString());
        } catch (InvalidCategoryOptionException | InvalidNameException e) {
            System.out.println(e.getMessage());
        }
        event.getTableView().refresh();
    }

    @FXML
    private void productOptionEdited(TableColumn.CellEditEvent<Product, String> event) {
        try {
            event.getRowValue().setCategoryOptions((CategoryOption) event.getTableColumn().getUserData(), event.getNewValue());
        } catch (InvalidCategoryOptionException | InvalidNameException e) {
            System.out.println(e.getMessage());
        }
        event.getTableView().refresh();
    }

    @FXML
    private void filter() {
        if (filterCategoryComboBox.getValue() != null) {
            ObservableList<CategoryOption> filterCategoryOptions = filterCategoryComboBox.getValue().getOptions();
            filterOptionComboBox.setItems(filterCategoryOptions);
            productTableView.getColumns().remove(1, productTableView.getColumns().size());
            filterCategoryOptions.forEach(categoryOption -> {
                TableColumn<Product, String> column = new TableColumn<>(categoryOption.getName());
                column.setCellValueFactory(cellDataFeatures ->
                    new SimpleStringProperty(cellDataFeatures.getValue().getCategoryOption(categoryOption))
                );
                column.setPrefWidth(96);
                // lagrer hvilken egenskap kolonnen tilhører, vi trenger den i productOptionEdited for å oppdatere riktig egenskap
                column.setUserData(categoryOption);
                column.setCellFactory(TextFieldTableCell.forTableColumn());
                column.setOnEditCommit(this::productOptionEdited);

                productTableView.getColumns().add(column);
            });

            filteredProductRegister.setAll(productRegister.stream()
                    .filter(product -> {
                        // hvis filteret er tomt vise vi alle produktene med valgt kategori
                        if (filterTextField.getText().isEmpty())
                            return product.getCategory().equals(filterCategoryComboBox.getValue());
                       else if (product.getCategoryOption(filterOptionComboBox.getValue()) != null)
                            return product.getCategory().equals(filterCategoryComboBox.getValue()) && product.getCategoryOption(filterOptionComboBox.getValue()).toLowerCase().contains(filterTextField.getText().toLowerCase());
                       else
                           return false;
                    }
            ).collect(Collectors.toCollection(FXCollections::observableArrayList)));


            TableColumn deleteButtonColumn = new TableColumn<>("Slett");
            Callback<TableColumn<?, String>, TableCell<?, String>> cellFactory = new DeleteButtonCellFactory(productTableView, productRegister, this::filter).cellFactory;
            deleteButtonColumn.setCellFactory(cellFactory);
            deleteButtonColumn.setPrefWidth(36.0);
            // replaces old column connected to the previously selected category
            if (productTableView.getColumns().get(productTableView.getColumns().size() - 1).getText().equals("Slett"))
                productTableView.getColumns().remove(1);

            productTableView.getColumns().add(deleteButtonColumn);
        } else {
            filteredProductRegister.clear();
        }
    }

    @FXML
    private void openCategory() {
        FileChooser fileChooser = new FileChooser();
        fileChooser.setTitle("Velg fil");
        fileChooser.getExtensionFilters().add(new FileChooser.ExtensionFilter("Serialiserte filer", "*.jobj"));
        fileChooser.setInitialDirectory(new File(System.getProperty("user.dir")));

        // Try/Catch sånn at du ikke får feilmedling nå du lukker "Åpne fil" vindu
        try {
            File selectedFile = fileChooser.showOpenDialog(new Stage());
            Path filePath = selectedFile.toPath();
            task = new Threads(filePath, Threads.METHOD.OPENJOBJ);
            task.setOnSucceeded(this::enableGUI);
            task.setOnFailed(this::enableGUI);
            Thread th = new Thread(task);
            th.setDaemon(true);
            disableGUI();
            th.start();
        } catch (Exception e) {
            System.out.println("e = " + e);
        }
    }

    @FXML
    private void saveCategory() {
        FileChooser fileChooser = new FileChooser();
        fileChooser.setTitle("Velg fil");
        fileChooser.getExtensionFilters().add(new FileChooser.ExtensionFilter("Serialiserte filer", "*.jobj"));
        fileChooser.setInitialDirectory(new File(System.getProperty("user.dir")));

        // Try/Catch sånn at du ikke får feilmedling nå du lukker "Lagre fil" vindu
        try {
            File selectedFile = fileChooser.showSaveDialog(new Stage());
            Path filePath = selectedFile.toPath();
            task = new Threads(filePath, Threads.METHOD.SAVEJOBJ);
            task.setOnSucceeded(this::enableGUI);
            task.setOnFailed(this::enableGUI);
            Thread th = new Thread(task);
            th.setDaemon(true);
            disableGUI();
            th.start();
        } catch (Exception e){
            System.out.println("e = " + e);
        }
    }
    @FXML
    public void openProduct() {
        FileChooser fileChooser = new FileChooser();
        fileChooser.setTitle("Velg fil");
        fileChooser.getExtensionFilters().add(new FileChooser.ExtensionFilter("CSV filer", "*.csv"));
        fileChooser.setInitialDirectory(new File(System.getProperty("user.dir")));

        try {
        File selectedFile = fileChooser.showOpenDialog(new Stage());
        Path filePath = selectedFile.toPath();
        task = new Threads(filePath, Threads.METHOD.OPENCSV);
        task.setOnSucceeded(this::enableGUI);
        task.setOnFailed(this::enableGUI);
        Thread th = new Thread(task);
        th.setDaemon(true);
        disableGUI();
        th.start();
        } catch (Exception e){
            System.out.println("e = " + e);
        }
    }

    @FXML
    private void saveProduct() {
        FileChooser fileChooser = new FileChooser();
        fileChooser.setTitle("Velg fil");
        fileChooser.getExtensionFilters().add(new FileChooser.ExtensionFilter("CSV filer", "*.csv"));
        fileChooser.setInitialDirectory(new File(System.getProperty("user.dir")));

        try{
        File selectedFile = fileChooser.showSaveDialog(new Stage());
        Path filePath = selectedFile.toPath();
        task = new Threads(filePath, Threads.METHOD.SAVECSV);
        task.setOnSucceeded(this::enableGUI);
        task.setOnFailed(this::enableGUI);
        Thread th = new Thread(task);
        th.setDaemon(true);
        disableGUI();
        th.start();
        } catch (Exception e){
            System.out.println("e = " + e);
        }
    }

    private void enableGUI(WorkerStateEvent e) {
        vbox.getChildren().forEach(node -> node.setDisable(false));
    }

    private void disableGUI() {
        vbox.getChildren().forEach(node -> node.setDisable(true));
    }
}
