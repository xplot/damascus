import os
import fileinput
import shutil


def environment_dict():
    imeet_dict = {}
    for variable in os.environ.keys():
        variablel = variable.lower()
        if "imeet." in variablel:
            imeet_dict[variablel.replace("damascus.", "")] = os.environ[variablel]
    return imeet_dict

def get_existing_env_variable_in_line(line, imeet_dict):
    for key in imeet_dict.keys():
        if key in line:
            return key
    return None

def replace_file_variables(filename):
    for line in fileinput.input(filename, inplace=True):
        env_variable = get_existing_env_variable_in_line(line, imeet_dict)
        if env_variable:
            print(line.replace(line[line.index(":"):-1], ":'" + imeet_dict[env_variable] + "',"))
        else:
            print(line)


imeet_dict = environment_dict()

replace_file_variables("src/Damascus.MessageChannel/config.json")
replace_file_variables("src/Damascus.Web/config.json")

