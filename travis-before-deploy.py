import os
import fileinput
import shutil
import json
import urllib

PREFIX = 'damascus_'
def environment_dict():
    imeet_dict = {}
    for variable in os.environ.keys():
        variablel = variable.lower()
        if PREFIX in variablel:
            imeet_dict[variable] = os.environ[variable]
    return imeet_dict

def create_variables_file(variables_file, imeet_dict):
    with open(variables_file, 'w') as f:
        f.write('{\n')
    	for x in imeet_dict:
            f.write('"%s" : "%s", \n' %( x, urllib.unquote(imeet_dict[x])))
        
        #last instruction so it doesnt contain coma
        f.write(' "1": "1"\n')
           
        f.write('}\n')
 
imeet_dict = environment_dict()
create_variables_file('config_variables', imeet_dict)