{{/* vim: set filetype=mustache: */}}
{{/*
Expand the name of the chart.
*/}}
{{- define "project.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "project.fullname" -}}
{{- if .Values.fullnameOverride -}}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- $name := default .Chart.Name .Values.nameOverride -}}
{{- if contains $name .Release.Name -}}
{{- .Release.Name | trunc 63 | trimSuffix "-" -}}
{{- else -}}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" -}}
{{- end -}}
{{- end -}}
{{- end -}}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "project.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{/*
Common labels (used by Gateway API HTTPRoute).
*/}}
{{- define "project.labels" -}}
helm.sh/chart: {{ include "project.chart" . }}
app.kubernetes.io/name: {{ include "project.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end -}}

{{/*
Fully-qualified container image reference.

This was previously hardcoded in deployment.yaml as
  docker.io/simplify9/<chart name>:<chart version>
which ignored the image.repository / image.tag values the CI pipeline has been
passing all along, and pointed at a registry that has received no push since
6.0.8 (Aug 2025). Images are published to ghcr.io, so that is the default now.
Both halves stay overridable for anyone who needs a different registry or tag.
*/}}
{{- define "project.image" -}}
{{- $image := .Values.image | default dict -}}
{{- $repo := $image.repository | default (printf "ghcr.io/simplify9/%s" .Chart.Name) -}}
{{- $tag := $image.tag | default .Chart.Version -}}
{{- printf "%s:%s" $repo $tag -}}
{{- end -}}
