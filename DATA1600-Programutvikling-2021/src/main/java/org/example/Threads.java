package org.example;

import javafx.collections.ObservableList;
import javafx.concurrent.Task;
import org.example.io.FileOpenerCSV;
import org.example.io.FileOpenerJobj;
import org.example.io.FileSaverCSV;
import org.example.io.FileSaverJobj;

import java.io.IOException;
import java.nio.file.Path;

import static javax.swing.JOptionPane.showMessageDialog;

class Threads extends Task<Object> {
    enum METHOD {
        OPENJOBJ,
        OPENCSV,
        SAVEJOBJ,
        SAVECSV
    };

    Path filePath;
    METHOD method;

    public Threads(Path filePath, METHOD method) {
        this.filePath = filePath;
        this.method = method;
    }
    @Override
    public Object call() {
        try {
            Thread.sleep(3000);

            switch (method) {
                case OPENJOBJ:
                    PrimaryController.categoryRegister.setAll(FileOpenerJobj.open(filePath));
                    break;
                case SAVEJOBJ:
                    FileSaverJobj.save(PrimaryController.categoryRegister, filePath);
                    break;
                /*case OPENCSV:
                    PrimaryController.productRegister.setAll(FileOpenerCSV.open(filePath));
                    break;*/
                case SAVECSV:
                    FileSaverCSV.save(PrimaryController.productRegister, filePath);
                    break;
            }
        } catch (IOException e) {
            showMessageDialog(null, e.getMessage());
            // hvis filutforskeren lukkes uten å åpne fil ignorerer vi avviket
        } catch (InterruptedException e) {
            showMessageDialog(null, "Feil i tråd: " + e.getMessage());
        } catch (NullPointerException ignored) {}
        return null;
    }
}