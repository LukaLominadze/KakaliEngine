def add_config_details(file_path, added_config):
    print('Adding "Distribution" configuration in ' + file_path)
    content = []
    with open(file_path, 'r') as proj:
        content_temp = proj.readlines()
        for line in content_temp:
            if '<Nullable>' in line:
                content.append(line)
                content.append('\n    <Configurations>Debug;Release;Distribution</Configurations>')
            elif '</Project>' in line:
                content.append(added_config)
                content.append(line)
            else:
                content.append(line)

    with open(file_path, 'w') as proj:
        proj.writelines(content)
    print(added_config)
    print('***')

distribution_config = ''
sandbox_config = ''

with open('vendor/bin/scripts/projectconfigextension.xml', 'r') as config_file:
    distribution_config = config_file.read()
config_file.close()
with open('vendor/bin/scripts/sandboxconfigentension.xml', 'r') as config_file:
    sandbox_config = config_file.read()
config_file.close()

sln_content = []
print('Adding "Distribution" configuration in Game2D.sln')
with open('Game2D.sln', 'r') as sln_file:
    sln_content_temp = sln_file.readlines()
    for line in sln_content_temp:
        if 'Release|Any CPU = Release|Any CPU' in line:
            sln_content.append('		\nDistribution|Any CPU = Distribution|Any CPU\n')
            sln_content.append(line)
        elif '.Release|Any CPU.Build.0 = Release|Any CPU' in line:
            guid = line.strip("{}")
            sln_content.append(line)
            sln_content.append('		\n{138C0E1E-28EA-455C-976A-EFAE736A7C5E}.Distribution|Any CPU.ActiveCfg = Distribution|Any CPU')
            sln_content.append('		\n{138C0E1E-28EA-455C-976A-EFAE736A7C5E}.Distribution|Any CPU.Build.0 = Distribution|Any CPU')
        else:
            sln_content.append(line)

with open('Game2D.sln', 'w') as sln_file:
    sln_file.writelines(sln_content)

add_config_details('Game2D/Game2D.csproj', distribution_config)
add_config_details('SandboxTK/SandboxTK.csproj', sandbox_config)