mkdir 'Innoactive'
mkdir 'Innoactive/Creator'
mkdir 'Innoactive/Creator/Components'

cd 'Innoactive/Creator'

git submodule add -b develop git@github.com:Innoactive/Creator.git Core

git submodule add -b develop git@github.com:Innoactive/VRTK-Interaction-Component.git 'Components/VRTK-Interactions'
git submodule add -b develop git@github.com:Innoactive/TextToSpeech-Component.git 'Components/Text-To-Speech'

git submodule add -b develop git@github.com:Innoactive/IA-Training-Template.git 'Basic-Template'

git submodule update --init --recursive
