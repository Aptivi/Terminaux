ROOT_DIR := $(shell dirname "$(realpath $(lastword $(MAKEFILE_LIST)))")
OUTPUTS  := \
	-name "bin" -or \
	-name "obj" -or \
	-name "docs"

.PHONY: all

# General use

all:
	$(MAKE) all-online BUILDARGS="$(BUILDARGS)"

all-online:
	$(MAKE) invoke-build ENVIRONMENT=Release BUILDARGS="$(BUILDARGS)"

dbg:
	$(MAKE) invoke-build ENVIRONMENT=Debug BUILDARGS="$(BUILDARGS)"

dbg-ci:
	$(MAKE) invoke-build ENVIRONMENT=Debug BUILDARGS="-p:ContinuousIntegrationBuild=true $(BUILDARGS)"

rel-ci:
	$(MAKE) invoke-build ENVIRONMENT=Release BUILDARGS="-p:ContinuousIntegrationBuild=true $(BUILDARGS)"

doc: invoke-doc-build

clean:
	find "$(ROOT_DIR)" -type d \( $(OUTPUTS) \) -print -exec rm -rf "{}" +

# Below targets specify functions for full build

invoke-build:
	bash tools/build.sh "$(ENVIRONMENT)" $(BUILDARGS)

invoke-doc-build:
	bash tools/docgen.sh
