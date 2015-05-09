import os
import fileinput
import shutil
import json

PREFIX = 'damascus_'
VARIABLES_FILE = 'config_variables'
pretty_name = lambda x: x.replace(PREFIX, "").replace('_','.')

def environment_dict(config_variables):
    with open(config_variables, 'r') as content_file:
        return json.load(content_file)

def get_existing_env_variable_in_line(line, imeet_dict):
    for key in imeet_dict.keys():
        if pretty_name(key) in line:
            return key
    return None

def replace_file_variables(filename):
    
    for line in fileinput.input(filename, inplace=True):
        env_variable = get_existing_env_variable_in_line(line, imeet_dict)
        if env_variable:
            line = "%s:'%s'," % ( pretty_name(env_variable),  imeet_dict[env_variable])
        print line 

imeet_dict = environment_dict(VARIABLES_FILE)

replace_file_variables("src/Damascus.MessageChannel/config.json")
replace_file_variables("src/Damascus.Web/config.json")

