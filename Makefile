ROOT_DIR := $(shell dirname "$(realpath $(lastword $(MAKEFILE_LIST)))")

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
	adt clean

# Below targets specify functions for full build

invoke-build:
	adt build -b "-p:Configuration=$(ENVIRONMENT) $(BUILDARGS)"

invoke-doc-build:
	adt gendocs
