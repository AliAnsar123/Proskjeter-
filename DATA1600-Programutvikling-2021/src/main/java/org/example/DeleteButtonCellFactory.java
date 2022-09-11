package org.example;

import javafx.collections.ObservableList;
import javafx.scene.control.Button;
import javafx.scene.control.TableCell;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableView;
import javafx.util.Callback;

public class DeleteButtonCellFactory {
    public Callback<TableColumn<?, String>, TableCell<?, String>> cellFactory;

    public DeleteButtonCellFactory(TableView<?> tableView, ObservableList<?> observableList, Runnable function) {
        cellFactory = new Callback<>() {
            @Override
            public TableCell<?, String> call(final TableColumn<?, String> param) {
                return new TableCell<>() {
                    private final Button deleteButton = new Button("X"); {
                        deleteButton.setOnAction(event -> {
                            observableList.remove(tableView.getItems().get(getIndex()));
                            tableView.refresh();
                            if (function != null)
                                function.run();
                        });
                    }
                    @Override
                    public void updateItem(String item, boolean empty) {
                        super.updateItem(item, empty);
                        if (empty)
                            setGraphic(null);
                        else
                            setGraphic(deleteButton);
                    }
                };
            }
        };
    }
}
