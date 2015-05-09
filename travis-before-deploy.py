import os
import fileinput
import shutil
import json

PREFIX = 'damascus_'
def environment_dict():
    imeet_dict = {}
    for variable in os.environ.keys():
        variablel = variable.lower()
        if PREFIX in variablel:
            imeet_dict[variable] = os.environ[variable]
    return imeet_dict

def create_variables_file(variables_file, imeet_dict):
    with open(variables_file, 'w+') as f:
    	f.write(json.dumps(imeet_dict))

imeet_dict = environment_dict()
create_variables_file('~/config_variables', imeet_dict)