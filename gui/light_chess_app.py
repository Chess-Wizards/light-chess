#!/usr/bin/python3

import sys

from PyQt5.QtWidgets import QApplication, QMainWindow


class LightChessApp(object):
    def run(self) -> None:
        app = QApplication(sys.argv)
        win = QMainWindow()
        win.setGeometry(400, 400, 300, 300)
        win.setWindowTitle("Light ⚡ Chess")
        win.show()
        sys.exit(app.exec_())
