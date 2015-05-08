import os
import fileinput
import shutil

PREFIX = 'damascus_'
pretty_name = lambda x: x.replace(PREFIX, "").replace('_','.')

def environment_dict():
    imeet_dict = {}
    for variable in os.environ.keys():
        variablel = variable.lower()
        if PREFIX in variablel:
            imeet_dict[variable] = os.environ[variable]
    return imeet_dict

def get_existing_env_variable_in_line(line, imeet_dict):
    for key in imeet_dict.keys():
        if pretty_name(key) in line:
            return key
    return None

def replace_file_variables(filename):
    
    for line in fileinput.input(filename, inplace=True):
        env_variable = get_existing_env_variable_in_line(line, imeet_dict)
        if env_variable:
            line = "%s:%s," % ( pretty_name(env_variable),  imeet_dict[env_variable])
        print line 

imeet_dict = environment_dict()

replace_file_variables("src/Damascus.MessageChannel/config.json")
replace_file_variables("src/Damascus.Web/config.json")

