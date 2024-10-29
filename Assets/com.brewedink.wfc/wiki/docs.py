from os import listdir, mkdir
import subprocess
from os.path import isfile, join
from shutil import copyfile, rmtree, which


src_path = '../Documentation~/CodeDocs'

code_path = 'CodeDocs'
dest_path = 'WaveFunctionCollapserDocs.wiki/' + code_path
md_ext = '.md'


# build documentation
print('BUILDING DOCS')
subprocess.run([which('dotnet'), 'build', '../Runtime/'])

# pull latest wiki
print('PULLING WIKI')
subprocess.Popen("git pull", cwd="./WaveFunctionCollapserDocs.wiki")

# capture current sidebar file content
sidebar_file = open('WaveFunctionCollapserDocs.wiki/_Sidebar.md', 'r')
original_sidebar_content = sidebar_file.read()
sidebar_file.close()

# add in code docs to sidebar
sidebar_file = open('WaveFunctionCollapserDocs.wiki/_Sidebar.md', 'a')
sidebar_file.write('\n\n# Code Docs\n')

try:
    rmtree(dest_path)
except OSError as e:
    pass
mkdir(dest_path)

onlyfiles = [f for f in listdir(src_path) if isfile(join(src_path, f))]


for file in onlyfiles:
    is_md = file[-(len(md_ext)):] == md_ext

    # filter for only mark down files...
    if not is_md:
        continue

    # remove extensions...
    fileName = file[:-len(md_ext)]

    if len(fileName) == 0:
        continue

    # get each part of the filename
    parts = fileName.split('_')

    # remove parameter args
    last_part = parts[-1]
    last_part_left_paren = last_part.find('(')

    indent = ''
    if len(parts) > 1:
        indent = ' '

    display = '.'.join(parts)
    destFileName = 'WaveFunctionCollapserDocs.wiki/' + code_path + '/' + fileName.replace('(', '').replace(')', '') + md_ext
    srcFilePath = src_path + '/' + fileName + md_ext
    copyfile(srcFilePath, destFileName)
    toc_line = (indent + '* [' + display + '](' + code_path + '/' + destFileName + ')')
    sidebar_file.write(toc_line + '\n')


sidebar_file.close()

print('RUNNING DOC PDF TOOL https://github.com/yakivmospan/github-wikito-converter')
try:
    subprocess.run([which('gwtc'), '-f', 'pdf', './WaveFunctionCollapserDocs.wiki/'])
finally:
    sidebar_file = open('WaveFunctionCollapserDocs.wiki/_Sidebar.md', 'w')
    sidebar_file.write(original_sidebar_content)
    sidebar_file.close()
